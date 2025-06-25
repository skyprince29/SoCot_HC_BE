using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.DTO.ParamDto;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Persons.Interfaces;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Services
{
    public class FacilityReferralService : Repository<Referral, Guid>, IFacilityReferralService
    {
        private readonly IPersonService _personService;
        private readonly ITransactionFlowHistoryService _transactionFlowHistoryService;
        public FacilityReferralService
            (AppDbContext context,
            IPersonService personService,
            ITransactionFlowHistoryService transactionFlowHistoryService
            ) : base(context)
        {
            _personService = personService;
            _transactionFlowHistoryService = transactionFlowHistoryService;
        }

        //Overloads method from Repository, Added facility to eager loading
        public override async Task<Referral?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var registry = await _dbSet
                .Include(pr => pr.FacilityReferredFrom)
                .Include(pr => pr.FacilityReferredTo)
                .Include(pr => pr.Personnel)
                .Include(pr => pr.AttendingPhysician)
                .Include(pr => pr.Status)
                .Include(pr => pr.ReferralServices)
                    .ThenInclude(dt => dt.Service)
                .FirstOrDefaultAsync(pr => pr.ReferralId == id, cancellationToken);
            return registry;
        }

        //private IQueryable<Referral> BuildFilteredQuery(GetPagedReferralParam request)
        //{
        //    var query = _dbSet
        //        .Include(i => i.ReferredFrom)
        //        .Include(pr => pr.ReferredTo)
        //        .Include(pr => pr.Personnel)
        //        .Include(pr => pr.AttendingPhysician)
        //        .Include(i => i.Status)
        //        .AsQueryable();

        //    // Required: Current department
        //    query = query.Where(t => t.ReferredFrom == request.ReferredFrom);


        //    var joinedQuery = query
        //    // Left Join for FromDepartmentId
        //    .GroupJoin(_context.Set<Department>(), // Your Department DbSet
        //               patientTrans => patientTrans.FromDepartmentId, // Key from PatientDepartmentTransaction
        //               department => department.DepartmentId, // Key from Department
        //               (patientTrans, fromDepartments) => new { patientTrans, fromDepartments }) // Result selector for GroupJoin
        //    .SelectMany(
        //        temp => temp.fromDepartments.DefaultIfEmpty(), // DefaultIfEmpty for Left Join
        //        (temp, fromDepartment) => new { temp.patientTrans, fromDepartment }) // Result selector for SelectMany
        //                                                                             // Left Join for DepartmentId (Current Department)
        //    .GroupJoin(_context.Set<Department>(), // Your Department DbSet
        //               temp => temp.patientTrans.DepartmentId, // Key from PatientDepartmentTransaction
        //               department => department.DepartmentId, // Key from Department
        //               (temp, currentDepartments) => new { temp.patientTrans, temp.fromDepartment, currentDepartments }) // Result selector for GroupJoin
        //    .SelectMany(
        //        temp => temp.currentDepartments.DefaultIfEmpty(), // DefaultIfEmpty for Left Join
        //        (temp, currentDepartment) => new // Project into an anonymous type
        //        {
        //            Referral = temp.patientTrans,
        //            FromDepartmentName = temp.fromDepartment != null ? temp.fromDepartment.DepartmentName : null, // Assuming Department.Name
        //            CurrentDepartmentName = currentDepartment != null ? currentDepartment.DepartmentName : null // Assuming Department.Name
        //        });


        //    if (request.FromDepartmentId.HasValue && request.FromDepartmentId.Value != Guid.Empty)
        //    {
        //        joinedQuery = joinedQuery.Where(t => t.Referral.FromDepartmentId == request.FromDepartmentId);
        //    }

        //    if (request.StatusId.HasValue)
        //    {
        //        joinedQuery = joinedQuery.Where(t => t.Referral.StatusId == request.StatusId.Value);
        //    }

        //    joinedQuery = joinedQuery.Where(t =>
        //        t.Referral.ReferralDateTime >= request.DateFrom.Date &&
        //        t.Referral.ReferralDateTime < request.DateTo.Date.AddDays(1));

        //    if (!string.IsNullOrWhiteSpace(request.Keyword))
        //    {
        //        string keyword = request.Keyword.Trim().ToLower();

        //        joinedQuery = joinedQuery.Where(t =>
        //            (t.Referral.PatientRegistry != null && EF.Functions.Like(t.Referral.PatientRegistry.Name, $"%{keyword}%")) ||
        //            (t.Referral.Status != null && EF.Functions.Like(t.Referral.Status.Name, $"%{keyword}%")));
        //    }


        //    var finalResult = joinedQuery.Select(x => new Referral
        //    {
        //        ReferralId = x.PatientTransaction.Id,
        //        PatientRegistryId = x.PatientTransaction.PatientRegistryId,
        //        PatientRegistry = x.PatientTransaction.PatientRegistry,
        //        FromDepartmentId = x.PatientTransaction.FromDepartmentId,
        //        DepartmentId = x.PatientTransaction.DepartmentId,
        //        TransactionDate = x.PatientTransaction.TransactionDate,
        //        ForwardedBy = x.PatientTransaction.ForwardedBy,
        //        AcceptedBy = x.PatientTransaction.AcceptedBy,
        //        Status = x.PatientTransaction.Status,
        //        IsCompleted = x.PatientTransaction.IsCompleted,
        //        IsActive = x.PatientTransaction.IsActive,
        //        Remarks = x.PatientTransaction.Remarks,
        //        FromDepartment = x.FromDepartmentName,
        //        Department = x.CurrentDepartmentName,
        //        ModuleId = x.PatientTransaction.ModuleId,
        //        StatusId = x.PatientTransaction.StatusId,

        //    });

        //    return finalResult;
        //}

        //public Task<int> CountAsync(GetPagedReferralParam request, CancellationToken cancellationToken = default)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<List<Referral>> GetAllWithPagingAsync(GetPagedReferralParam request, CancellationToken cancellationToken = default)
        //{
        //    throw new NotImplementedException();
        //}

        private Referral DTOToModel(FacilityReferralDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var referralId = dto.ReferralId;
            var statusId = dto.StatusId;
            return new Referral
            {
                ReferralId = referralId == Guid.Empty ? Guid.Empty : referralId,
                TempRefId = dto.TempRefId,
                Complains = dto.Complains ?? string.Empty,
                Reason = dto.Reason ?? string.Empty,
                Remarks = dto.Remarks,
                Diagnosis = dto.Diagnosis,
                ReferredTo = dto.ReferredTo ?? 0,
                ReferredFrom = dto.ReferredFrom ?? 0,
                ReferralNo = dto.ReferralNo ?? string.Empty,
                ReferralDateTime = dto.ReferralDateTime ?? DateTime.Now,
                DischargeInstructions = dto.DischargeInstructions,
                PersonnelId = dto.PersonnelId,
                AttendingPhysicianId = dto.AttendingPhysicianId,
                IsUrgent = dto.IsUrgent,
                CreatedBy = dto.CreatedBy,
                CreatedDate = dto.CreatedDate,
                StatusId = statusId.HasValue ? statusId.Value : (byte)0,
                ReferralServices = dto.ReferralServiceIds?
                    .Select(serviceId => new SoCot_HC_BE.Model.ReferralService
                    {
                        Id = Guid.NewGuid(),
                        ServiceId = serviceId,
                        ReferralId = referralId == Guid.Empty ? Guid.Empty : referralId,
                    }).ToList() ?? new List<SoCot_HC_BE.Model.ReferralService>(),
            };
        }

        public async Task<Referral> SaveReferralAsync(FacilityReferralDto facilityReferralDto, CancellationToken cancellationToken = default)
        {
            using (var dbtransaction = await _context.Database.BeginTransactionAsync(cancellationToken))
            {
                ValidateFields(facilityReferralDto);
                try
                {
                    Referral referral = DTOToModel(facilityReferralDto);
                    UserData user = _context.GetCurrentUser();

                    // Determine if new or existing
                    bool isNew = referral.ReferralId == Guid.Empty;
                    if (isNew)
                    {
                        // Create a new Referral
                        referral.ReferralId = Guid.NewGuid();
                        string timestamp = DateTime.Now.ToString("yyMMdd-HHmmss");
                        referral.ReferralNo = $"REF008-{timestamp}";

                        await _transactionFlowHistoryService.StarterLogAsync(referral, cancellationToken);
                        await _dbSet.AddAsync(referral, cancellationToken);
                    }
                    else
                    {
                        var existing = await _dbSet
                            .Include(d => d.ReferralServices)
                            .FirstOrDefaultAsync(d => d.ReferralId == referral.ReferralId, cancellationToken);
                        if (existing == null)
                            throw new Exception("Referral not found.");

                        // Replace all fields
                        _context.Entry(existing).CurrentValues.SetValues(referral);
                        AddOrUpdateReferralService(existing, facilityReferralDto.ReferralServiceIds);
                        _context.Update(existing);
                    }
                    await _context.SaveChangesAsync(cancellationToken); // Only save once, here
                    await dbtransaction.CommitAsync(cancellationToken);
                    return referral;
                }
                catch (Exception ex)
                {
                    // Rollback if any error occurs
                    await dbtransaction.RollbackAsync();
                    throw new Exception("Failed to save Patient Department transaction and log: " + ex.Message, ex);
                }
            }
        }

        private void AddOrUpdateReferralService(Referral existingReferral, List<Guid> newServiceIds)
        {
            var referralServiceSet = _context.Set<SoCot_HC_BE.Model.ReferralService>();

            // Remove all existing department-type relationships
            foreach (var existing in existingReferral.ReferralServices.ToList())
            {
                referralServiceSet.Remove(existing);
            }

            // Add new department-type relationships
            foreach (var typeId in newServiceIds)
            {
                referralServiceSet.Add(new Model.ReferralService
                {
                    ReferralId = existingReferral.ReferralId,
                    ServiceId = typeId
                });
            }
        }

        private void ValidateFields(FacilityReferralDto referral)
        {
            var errors = new Dictionary<string, List<string>>();

            ValidationHelper.IsRequired(errors, nameof(referral.ReferralNo), referral.ReferralNo, "Referral No");
            ValidationHelper.IsRequired(errors, nameof(referral.Complains), referral.Complains, "Complains");
            ValidationHelper.IsRequired(errors, nameof(referral.Reason), referral.Reason, "Reason for Referral");
            ValidationHelper.IsRequired(errors, nameof(referral.ReferredFrom), referral.ReferredFrom, "Referred From");
            ValidationHelper.IsRequired(errors, nameof(referral.ReferredTo), referral.ReferredTo, "Referred To");
            ValidationHelper.IsRequired(errors, nameof(referral.ReferralDateTime), referral.ReferralDateTime, "Referral Date");

            if (referral.ReferralServiceIds == null || !referral.ReferralServiceIds.Any())
                ValidationHelper.AddError(errors, nameof(referral.ReferralServiceIds), "At least one service must be selected.");

            if (errors.Any())
                throw new ModelValidationException("Validation failed", errors);
        }

    }
}
