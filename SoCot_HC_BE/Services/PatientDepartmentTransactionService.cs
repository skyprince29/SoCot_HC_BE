using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.DTO.ParamDto;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Model.Enums;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Services
{
    public class PatientDepartmentTransactionService : Repository<PatientDepartmentTransaction, Guid>, IPatientDepartmentTransactionService
    {
        private readonly ITransactionFlowHistoryService _transactionFlowHistoryService;
        private readonly IModuleStatusFlowService _moduleStatusFlowService;
        public PatientDepartmentTransactionService(
            AppDbContext context, 
            ITransactionFlowHistoryService transactionFlowHistoryService,
            IModuleStatusFlowService moduleStatusFlowService) 
            : base(context)
        {
            _transactionFlowHistoryService = transactionFlowHistoryService;
            _moduleStatusFlowService = moduleStatusFlowService;
        }

        public override async Task<PatientDepartmentTransaction?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {

            var query = _dbSet
             .Include(i => i.PatientRegistry)
             .Include(i => i.Status)
             .AsQueryable();

            query = query.Where(t => t.Id == id);


            var joinedQuery = query
          .GroupJoin(_context.Set<Department>(),
                     patientTrans => patientTrans.FromDepartmentId,
                     department => department.DepartmentId,
                     (patientTrans, fromDepartments) => new { patientTrans, fromDepartments })
          .SelectMany(
              temp => temp.fromDepartments.DefaultIfEmpty(),
              (temp, fromDepartment) => new { temp.patientTrans, fromDepartment })
          .GroupJoin(_context.Set<Department>(),
                     temp => temp.patientTrans.DepartmentId,
                     department => department.DepartmentId,
                     (temp, currentDepartments) => new { temp.patientTrans, temp.fromDepartment, currentDepartments })
          .SelectMany(
              temp => temp.currentDepartments.DefaultIfEmpty(),
              (temp, currentDepartment) => new
              {
                  PatientTransaction = temp.patientTrans,
                  FromDepartmentName = temp.fromDepartment != null ? temp.fromDepartment.DepartmentName : "",
                  CurrentDepartmentName = currentDepartment != null ? currentDepartment.DepartmentName : ""
              });

            var finalResult = joinedQuery.Select(x => new PatientDepartmentTransaction
            {
                Id = x.PatientTransaction.Id,
                PatientRegistryId = x.PatientTransaction.PatientRegistryId,
                PatientRegistry = x.PatientTransaction.PatientRegistry,
                FromDepartmentId = x.PatientTransaction.FromDepartmentId,
                DepartmentId = x.PatientTransaction.DepartmentId,
                TransactionDate = x.PatientTransaction.TransactionDate,
                ForwardedBy = x.PatientTransaction.ForwardedBy,
                AcceptedBy = x.PatientTransaction.AcceptedBy,
                Status = x.PatientTransaction.Status,
                IsCompleted = x.PatientTransaction.IsCompleted,
                IsActive = x.PatientTransaction.IsActive,
                Remarks = x.PatientTransaction.Remarks,
                FromDepartment = x.FromDepartmentName,
                Department = x.CurrentDepartmentName,
                ModuleId = x.PatientTransaction.ModuleId,
                StatusId = x.PatientTransaction.StatusId,
            });

            return await finalResult.FirstOrDefaultAsync();

        }

        private IQueryable<PatientDepartmentTransaction> BuildFilteredQuery(GetPagedPDTRequestParam request)
        {
            var query = _dbSet
                .Include(i => i.PatientRegistry)
                .Include(i => i.Status)
                .AsQueryable();

            if (!request.isForForward) {
                // Required: Current department
                query = query.Where(t => t.DepartmentId == request.CurrentDepartmentId);

            }

            var joinedQuery = query
            // Left Join for FromDepartmentId
            .GroupJoin(_context.Set<Department>(), // Your Department DbSet
                       patientTrans => patientTrans.FromDepartmentId, // Key from PatientDepartmentTransaction
                       department => department.DepartmentId, // Key from Department
                       (patientTrans, fromDepartments) => new { patientTrans, fromDepartments }) // Result selector for GroupJoin
            .SelectMany(
                temp => temp.fromDepartments.DefaultIfEmpty(), // DefaultIfEmpty for Left Join
                (temp, fromDepartment) => new { temp.patientTrans, fromDepartment }) // Result selector for SelectMany
                                                                                     // Left Join for DepartmentId (Current Department)
            .GroupJoin(_context.Set<Department>(), // Your Department DbSet
                       temp => temp.patientTrans.DepartmentId, // Key from PatientDepartmentTransaction
                       department => department.DepartmentId, // Key from Department
                       (temp, currentDepartments) => new { temp.patientTrans, temp.fromDepartment, currentDepartments }) // Result selector for GroupJoin
            .SelectMany(
                temp => temp.currentDepartments.DefaultIfEmpty(), // DefaultIfEmpty for Left Join
                (temp, currentDepartment) => new // Project into an anonymous type
                {
                    PatientTransaction = temp.patientTrans,
                    FromDepartmentName = temp.fromDepartment != null ? temp.fromDepartment.DepartmentName : null, // Assuming Department.Name
                    CurrentDepartmentName = currentDepartment != null ? currentDepartment.DepartmentName : null // Assuming Department.Name
                });


            if (request.FromDepartmentId.HasValue && request.FromDepartmentId.Value != Guid.Empty)
            {
                joinedQuery = joinedQuery.Where(t => t.PatientTransaction.FromDepartmentId == request.FromDepartmentId);
            }

            if (request.StatusId.HasValue)
            {
                joinedQuery = joinedQuery.Where(t => t.PatientTransaction.StatusId == request.StatusId.Value);
            }

            joinedQuery = joinedQuery.Where(t =>
                t.PatientTransaction.TransactionDate >= request.DateFrom.Date &&
                t.PatientTransaction.TransactionDate < request.DateTo.Date.AddDays(1));

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                string keyword = request.Keyword.Trim().ToLower();

                joinedQuery = joinedQuery.Where(t =>
                    (t.PatientTransaction.PatientRegistry != null && EF.Functions.Like(t.PatientTransaction.PatientRegistry.Name, $"%{keyword}%")) ||
                    (t.PatientTransaction.Status != null && EF.Functions.Like(t.PatientTransaction.Status.Name, $"%{keyword}%"))); 
            }


            var finalResult = joinedQuery.Select(x => new PatientDepartmentTransaction
            {
                Id = x.PatientTransaction.Id,
                PatientRegistryId = x.PatientTransaction.PatientRegistryId,
                PatientRegistry = x.PatientTransaction.PatientRegistry, 
                FromDepartmentId = x.PatientTransaction.FromDepartmentId,
                DepartmentId = x.PatientTransaction.DepartmentId,
                TransactionDate = x.PatientTransaction.TransactionDate,
                ForwardedBy = x.PatientTransaction.ForwardedBy,
                AcceptedBy = x.PatientTransaction.AcceptedBy,
                Status = x.PatientTransaction.Status,
                IsCompleted = x.PatientTransaction.IsCompleted,
                IsActive = x.PatientTransaction.IsActive,
                Remarks = x.PatientTransaction.Remarks,
                FromDepartment = x.FromDepartmentName,
                Department = x.CurrentDepartmentName,
                ModuleId = x.PatientTransaction.ModuleId, 
                StatusId = x.PatientTransaction.StatusId, 

            });

            return finalResult;
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

            var dto = new UpdateStatusDto
            {
                TransactionId = Id,
                ModuleId = (int)ModuleEnum.PatientDepartmentTransaction, // Make sure ModuleId exists on the transaction
                StatusId = 9,
                Remarks = "Marked as accepted and set to On-going."
            };
            await _transactionFlowHistoryService.UpdateStatusEntityAsync<PatientDepartmentTransaction>(
                             transaction,
                             dto,
                             isSave: false,
                             cancellationToken
                         );
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<PatientDepartmentTransaction> CreateTransactionAsync(
            PatientForwardingDto dto,
            CancellationToken cancellationToken = default)
        {
            using (var dbtransaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {

                ValidateFields(dto); // validate before proceeding
                try
                {
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
                    await _dbSet.AddAsync(transaction, cancellationToken);


                    var currentPDTId = dto.CurrentPatientDepartmentTransactionId;
                    var statusId = dto.StatusId;
                    if (currentPDTId.HasValue && statusId.HasValue)
                    {
                        var currentPDT = await _dbSet.FirstOrDefaultAsync(t => t.Id == currentPDTId, cancellationToken);
                        var updateStatusDto = new UpdateStatusDto
                        {
                            TransactionId = currentPDTId.Value,
                            ModuleId = transaction.ModuleId, // Make sure ModuleId exists on the transaction
                            StatusId = statusId.Value,
                            Remarks = dto.Remarks
                        };
                        if (currentPDT != null)
                        {
                            await _transactionFlowHistoryService.UpdateStatusEntityAsync<PatientDepartmentTransaction>(
                              currentPDT,
                              updateStatusDto,
                              isSave: false,
                              cancellationToken
                          );
                        }
                      
                    }

                    await _transactionFlowHistoryService.StarterLogAsync(transaction, cancellationToken);

                    await _context.SaveChangesAsync(cancellationToken); // Only save once, here
                    await dbtransaction.CommitAsync(cancellationToken);
                    return transaction;
                }
                catch (Exception ex)
                {
                    // Rollback if any error occurs
                    await dbtransaction.RollbackAsync();
                    throw new Exception("Failed to save Patient Department transaction and log: " + ex.Message, ex);
                }
            }
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

            if (dto.CurrentPatientDepartmentTransactionId.HasValue)
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

        public async Task<PatientDeptTransVitalSignsDto> GetPatientDeptmentTransactionVitalsignAsync(Guid id, CancellationToken cancellationToken = default)
        {

            PatientDeptTransVitalSignsDto dto = new PatientDeptTransVitalSignsDto();

          PatientDepartmentTransaction? patientDepartmentTransaction = await _dbSet
                .Include(p => p.PatientRegistry)
                .Include(p => p.Status)
                .FirstOrDefaultAsync(i => i.Id == id);
            
            if (patientDepartmentTransaction != null)
            {
                dto.PatientDepartmentTransaction = patientDepartmentTransaction;
            }
            else
            {
                return dto;

            }

            var vitalSignIds = await _context.Set<VitalSignReference>()
                .Where(vref => ((int)vref.VitalSignReferenceType) == (int)VitalSignReferenceType.PatientDepartmentTransaction
                && vref.ReferenceId == patientDepartmentTransaction.Id)
                .Select(vref => vref.VitalSignId) // Select only the IDs
                .Distinct() // Get unique IDs to prevent duplicate VitalSign fetches
                .ToListAsync(cancellationToken);

            if (vitalSignIds.Any()) // Only query if there are IDs to search for
            {
                var vitalSigns = await _context.Set<VitalSign>()
                 .Where(vs => vitalSignIds.Contains(vs.VitalSignId))
                 .OrderByDescending(vs => vs.CreatedDate) // <--- ADDED THIS LINE
                 .ToListAsync(cancellationToken);

                // 4. Add the fetched VitalSigns to the DTO's list.
                //    Assuming dto.VitalSigns is initialized as new List<VitalSign>()
                dto.VitalSigns = vitalSigns;
            }





            return dto;

        }
    }
}
