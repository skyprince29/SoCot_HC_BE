using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Model.BaseModels;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;

namespace SoCot_HC_BE.Services
{
    public class TransactionFlowHistoryService : Repository<TransactionFlowHistory, Guid>, ITransactionFlowHistoryService
    {
        private readonly IModuleStatusFlowService _moduleStatusFlowService;
        private readonly ModuleServiceMapper _moduleServiceMapper;

        public TransactionFlowHistoryService(AppDbContext context, IModuleStatusFlowService moduleStatusFlowService,
            ModuleServiceMapper moduleServiceMapper) : base(context)
        {
            _moduleStatusFlowService = moduleStatusFlowService;
            _moduleServiceMapper = moduleServiceMapper;
        }

        public async Task StarterLogAsync<T>(T entity, CancellationToken cancellationToken = default)
        where T : IBaseTransaction
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            //if (await IsDuplicateLoggedStatus(entity.TransactionId, entity.ModuleId, cancellationToken))
            //    return;

            var firstStatus = await _moduleStatusFlowService.GetFirstStatusAsync(entity.ModuleId, cancellationToken);
            if (firstStatus == null)
                throw new Exception("First status not configured for module.");

            entity.StatusId = firstStatus.Id;

            bool isComplete = await _moduleStatusFlowService.IsCompleteStatusAsync(
                entity.ModuleId,
                entity.StatusId,
                cancellationToken
            );

            var user = _context.GetCurrentUser();

            var history = CreateLogEntry(
                user.UserId,
                entity.ModuleId,
                entity.TransactionId,
                null,
                entity.StatusId,
                null,
                isComplete
            );

            await _dbSet.AddAsync(history, cancellationToken);
        }

        public async Task UpdateStatusAsync(UpdateStatusDto dto, CancellationToken cancellationToken = default)
        {
            if (dto.StatusId == null)
                throw new ArgumentException("StatusId cannot be null.");

            var moduleServiceObj = _moduleServiceMapper.GetServiceByModuleId(dto.ModuleId);
            if (moduleServiceObj == null)
                throw new Exception("Unsupported module.");

            dynamic moduleService = moduleServiceObj;
            var entity = await moduleService.GetAsync(dto.TransactionId, cancellationToken);

            if (entity == null)
                throw new Exception("Entity not found.");

            byte currentStatus = entity.StatusId;
            byte newStatus = dto.StatusId.Value;

            if (currentStatus == newStatus)
                throw new Exception("The new status is the same as the current status.");
            else if (await _moduleStatusFlowService.IsCompleteStatusAsync(dto.ModuleId, currentStatus, cancellationToken))
                throw new Exception("Cannot update status because the transaction is already marked as complete.");

            entity.StatusId = newStatus;

            await moduleService.UpdateAsync(entity, cancellationToken);

            bool isComplete = await _moduleStatusFlowService.IsCompleteStatusAsync(dto.ModuleId, newStatus, cancellationToken);

            var user = _context.GetCurrentUser();

            var log = CreateLogEntry(
                user.UserId,
                dto.ModuleId,
                dto.TransactionId,
                currentStatus,
                newStatus,
                dto.Remarks,
                isComplete
            );

            await _dbSet.AddAsync(log, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task<bool> IsDuplicateLoggedStatus(Guid transactionId, int moduleId, CancellationToken cancellationToken)
        {
            return await _dbSet.AnyAsync(x =>
                x.TransactionId == transactionId &&
                x.ModuleId == moduleId &&
                x.PreviousStatusId == null,
                cancellationToken);
        }

        private TransactionFlowHistory CreateLogEntry(Guid userId, int moduleId, Guid transactionId,
             byte? previousStatusId, byte currentStatusId, string? remarks, bool isComplete)
        {
            return new TransactionFlowHistory
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Date = DateTime.UtcNow,
                ModuleId = moduleId,
                TransactionId = transactionId,
                PreviousStatusId = previousStatusId,
                CurrentStatusId = currentStatusId,
                Remarks = remarks,
                IsComplete = isComplete
            };
        }


    }
}
