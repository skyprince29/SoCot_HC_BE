using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO;
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

        public async Task<List<PatientDepartmentTransaction>> GetAllWithPagingAsync(
            Guid fromDepartmentId,
            Guid currentDepartmentId,
            int pageNo,
            int limit,
            string? keyword = null,
            byte? statusId = null,
            CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();

            // Filter by fromDepartmentId
             query = query
                .Include(i => i.PatientRegistry)
                .Include(i => i.Status)
                .Where(t => t.FromDepartmentId == fromDepartmentId);

            // Filter by currentDepartmentId optional

            if (currentDepartmentId != Guid.Empty)
            {
                query = query.Where(t => t.DepartmentId == currentDepartmentId);
            }

            // Optional filter by StatusId
            if (statusId.HasValue)
            {
                query = query.Where(t => t.StatusId == statusId.Value);
            }

            return await query
                .Skip((pageNo - 1) * limit)
                .Take(limit)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(
            Guid fromDepartmentId,
            Guid currentDepartmentId,
            string? keyword = null,
            byte? statusId = null,
            CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();

            // Filter by fromDepartmentId
            query = query
               .Include(i => i.PatientRegistry)
               .Include(i => i.Status)
               .Where(t => t.FromDepartmentId == fromDepartmentId);

            // Filter by currentDepartmentId optional

            if (currentDepartmentId != Guid.Empty)
            {
                query = query.Where(t => t.DepartmentId == currentDepartmentId);
            }

            // Optional filter by StatusId
            if (statusId.HasValue)
            {
                query = query.Where(t => t.StatusId == statusId.Value);
            }

            return await query.CountAsync(cancellationToken); // Pass the CancellationToken here
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
                FromDepartmentId = dto.FromDepartmentId,
                DepartmentId = dto.CurrentDepartmentId,
                ForwardedBy = user.UserId,
                Remarks = dto.Remarks,
                TransactionDate = DateTime.Now,
                StatusId = 0,
                IsCompleted = false,
                IsActive = true
            };

            await _transactionFlowHistoryService.StarterLogAsync(transaction, cancellationToken);

            await _context.PatientDepartmentTransaction.AddAsync(transaction, cancellationToken);
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
                Guid fromDeptId = dto.FromDepartmentId;

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
