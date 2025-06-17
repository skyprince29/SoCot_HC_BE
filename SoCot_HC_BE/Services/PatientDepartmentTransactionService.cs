using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.DTO.ParamDto;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Services
{
    public class PatientDepartmentTransactionService : Repository<PatientDepartmentTransaction, Guid>, IPatientDepartmentTransactionService
    {

        private readonly ITransactionFlowHistoryService _transactionFlowHistoryService;
        public PatientDepartmentTransactionService(AppDbContext context, ITransactionFlowHistoryService transactionFlowHistoryService) : base(context)
        {
            _transactionFlowHistoryService = transactionFlowHistoryService;
        }

        public override async Task<PatientDepartmentTransaction?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // You can include related data here if needed, like navigation properties
            var facility = await _dbSet
                .Include(f => f.PatientRegistry) // Example: include navigation property if needed
                .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

            return facility;
        }

        private IQueryable<PatientDepartmentTransaction> BuildFilteredQuery(GetPagedPDTRequestParam request)
        {
            var query = _dbSet
                .Include(i => i.PatientRegistry)
                .Include(i => i.Status)
                .AsQueryable();

            // Required: Current department
            query = query.Where(t => t.DepartmentId == request.CurrentDepartmentId);

            // Optional: FromDepartmentId
            if (request.FromDepartmentId.HasValue && request.FromDepartmentId.Value != Guid.Empty)
            {
                query = query.Where(t => t.FromDepartmentId == request.FromDepartmentId);
            }

            // Optional: StatusId
            if (request.StatusId.HasValue)
            {
                query = query.Where(t => t.StatusId == request.StatusId.Value);
            }

            // Required: Date filter (date only, inclusive)
            query = query.Where(t =>
                t.TransactionDate >= request.DateFrom.Date &&
                t.TransactionDate < request.DateTo.Date.AddDays(1));

            // Optional: Keyword
            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                string keyword = request.Keyword.Trim().ToLower();

                query = query.Where(t =>
                    (t.PatientRegistry != null && EF.Functions.Like(t.PatientRegistry.Name, $"%{keyword}%")) ||
                    (t.Status != null && EF.Functions.Like(t.Status.Name, $"%{keyword}%")));
            }

            return query;
        }

        public async Task<List<PatientDepartmentTransaction>> GetAllWithPagingAsync(
            GetPagedPDTRequestParam request,
            CancellationToken cancellationToken = default)
        {
            var query = BuildFilteredQuery(request);

            return await query
                .OrderByDescending(t => t.TransactionDate)
                .Skip((request.PageNo - 1) * request.Limit)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(
            GetPagedPDTRequestParam request,
            CancellationToken cancellationToken = default)
        {
            var query = BuildFilteredQuery(request);
            return await query.CountAsync(cancellationToken);
        }

        public async Task<bool> UpdateAcceptedByAsync(Guid Id, Guid acceptedByUserId, CancellationToken cancellationToken = default)
        {
            var transaction = await _dbSet.FirstOrDefaultAsync(t => t.Id == Id, cancellationToken);

            if (transaction == null)
                return false;

            UserData user = _context.GetCurrentUser();

            transaction.AcceptedBy = user.UserId !=  Guid.Empty ? user.UserId  : acceptedByUserId;
            await _context.SaveChangesAsync(cancellationToken);

            // Now call UpdateStatusAsync to set status to "On-going" (ID = 9)
            var dto = new UpdateStatusDto
            {
                TransactionId = Id,
                ModuleId = transaction.ModuleId, // Make sure ModuleId exists on the transaction
                StatusId = 9,
                Remarks = "Marked as accepted and set to On-going."
            };

            await _transactionFlowHistoryService.UpdateStatusAsync(dto, cancellationToken); // calling the method

            return true;
        }

        public async Task<PatientDepartmentTransaction> CreateTransactionAsync(
            PatientForwardingDto dto,
            CancellationToken cancellationToken = default)
        {
            ValidateFields(dto); // validate before proceeding
            UserData user = _context.GetCurrentUser();

            var transaction = new PatientDepartmentTransaction
            {
                Id = Guid.NewGuid(),
                PatientRegistryId = dto.PatientRegistryId,
                FromDepartmentId = dto.FromDepartmentId.HasValue ? dto.FromDepartmentId : null,
                DepartmentId = dto.CurrentDepartmentId,
                ForwardedBy = user.UserId,
                Remarks = dto.Remarks,
                TransactionDate = DateTime.Now,
                StatusId = 0,
                IsCompleted = false,
                IsActive = true
            };

            await _transactionFlowHistoryService.StarterLogAsync(transaction, cancellationToken);

            await AddAsync(transaction, cancellationToken);
            return transaction;
        }

        private void ValidateFields(PatientForwardingDto dto)
        {
            var errors = new Dictionary<string, List<string>>();

            string patientRegistryCN = nameof(dto.PatientRegistryId);
            Guid patientRegistryId = dto.PatientRegistryId;

            ValidationHelper.IsRequired(errors, patientRegistryCN, patientRegistryId, "Patient Registry");
            bool patientExists = _context.PatientRegistry.Any(p => p.PatientRegistryId == patientRegistryId);
            if (!patientExists && patientRegistryId != Guid.Empty)
            {
                ValidationHelper.AddError(errors, patientRegistryCN, "Patient Registry does not exist.");
            }

            if (dto.IsTransfer)
            {
                string fromDeptCN = nameof(dto.FromDepartmentId);
                Guid fromDeptId = dto.FromDepartmentId.HasValue ? dto.FromDepartmentId.Value : Guid.Empty;
                ValidationHelper.IsRequired(errors, fromDeptCN, fromDeptId, "Previous Department");
                bool fromDeptExists = _context.Department.Any(d => d.DepartmentId == fromDeptId);
                if (!fromDeptExists && fromDeptId != Guid.Empty)
                {
                    ValidationHelper.AddError(errors, fromDeptCN, "Previous Department is invalid.");
                }
            }

            string currentDeptCN = nameof(dto.CurrentDepartmentId);
            Guid currentDeptId = dto.CurrentDepartmentId;

            ValidationHelper.IsRequired(errors, currentDeptCN, currentDeptId, "Department");
            bool deptExists = _context.Department.Any(d => d.DepartmentId == currentDeptId);
            if (!deptExists && currentDeptId != Guid.Empty)
            {
                ValidationHelper.AddError(errors, currentDeptCN, "Department is invalid.");
            }

            string remarksCN = nameof(dto.Remarks);
            string remarks = dto.Remarks?.Trim() ?? "";
            ValidationHelper.IsRequired(errors, remarksCN, remarks, "Remarks");

            if (errors.Any())
                throw new ModelValidationException("Validation failed", errors);
        }
    }
}
