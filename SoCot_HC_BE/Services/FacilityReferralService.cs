using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.DTO.ParamDto;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Model.Enums;
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
        private readonly IVitalSignService _vitalSignService;
        public FacilityReferralService
            (AppDbContext context,
            IPersonService personService,
            IVitalSignService vitalSignService,
            ITransactionFlowHistoryService transactionFlowHistoryService
            ) : base(context)
        {
            _personService = personService;
            _vitalSignService = vitalSignService;
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
                .Include(pr => pr.Person)
                .Include(pr => pr.Status)
                .Include(pr => pr.ReferralServices)
                    .ThenInclude(dt => dt.Service)
                .FirstOrDefaultAsync(pr => pr.ReferralId == id, cancellationToken);
            return registry;
        }

        private IQueryable<Referral> BuildFilteredReferralQuery(GetPagedReferralParam request)
        {
            var query = _dbSet
                .Include(r => r.Status)
                .Include(r => r.FacilityReferredFrom)
                .Include(r => r.FacilityReferredTo)
                .Include(pr => pr.Person)
                .Include(r => r.Personnel)
                .Include(r => r.AttendingPhysician)
                .AsQueryable();
            // Optional: ReferredFrom
            if (request.ReferredFrom.HasValue)
            {
                query = query.Where(r => r.ReferredFrom == request.ReferredFrom.Value);
            }
            
            // Optional: ReferredTo
            if (request.ReferredTo.HasValue)
            {
                query = query.Where(r => r.ReferredTo == request.ReferredTo.Value);
            }

            // Optional: Status
            if (request.StatusId.HasValue)
            {
                query = query.Where(r => r.StatusId == request.StatusId.Value);
            }

            if (request.DateFrom.HasValue && request.DateTo.HasValue)
            {
                
            }
            else if (request.DateFrom.HasValue)
            {
                query = query.Where(r =>
                    r.ReferralDateTime >= request.DateFrom.Value.Date &&
                    r.ReferralDateTime < request.DateFrom.Value.Date.AddDays(1));
            }
            else if (request.DateTo.HasValue)
            {
                query = query.Where(r =>
                    r.ReferralDateTime >= request.DateTo.Value.Date &&
                    r.ReferralDateTime < request.DateTo.Value.Date.AddDays(1));
            }

            // Optional: Keyword search
            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                string keyword = request.Keyword.Trim().ToLower();
                query = query.Where(r =>
                    EF.Functions.Like(r.Complains.ToLower(), $"%{keyword}%") ||
                    EF.Functions.Like(r.Reason.ToLower(), $"%{keyword}%") ||
                    EF.Functions.Like(r.ReferralNo.ToLower(), $"%{keyword}%") ||
                    (r.Person != null && (
                        EF.Functions.Like(r.Person.Fullname.ToLower(), $"%{keyword}%") ||
                        EF.Functions.Like(r.Person.Completename.ToLower(), $"%{keyword}%")
                    ))
                );
            }
            return query;
        }

        public async Task<List<Referral>> GetAllWithPagingAsync(
            GetPagedReferralParam request,
            CancellationToken cancellationToken = default)
        {
            var query = BuildFilteredReferralQuery(request);

            return await query
                .OrderByDescending(r => r.ReferralDateTime)
                .Skip((request.PageNo - 1) * request.Limit)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(
            GetPagedReferralParam request,
            CancellationToken cancellationToken = default)
        {
            var query = BuildFilteredReferralQuery(request);
            return await query.CountAsync(cancellationToken);
        }

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
                PersonId = dto.PersonId, 
                ReferralServices = dto.ReferralServiceIds
                    .Select(serviceId => new SoCot_HC_BE.Model.ReferralService
                    {
                        Id = Guid.NewGuid(),
                        ServiceId = serviceId,
                        ReferralId = referralId == Guid.Empty ? Guid.Empty : referralId,
                    }).ToList(),
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
                    facilityReferralDto.VitalSign.ReferenceId = referral.ReferralId;
                    facilityReferralDto.VitalSign.VitalSignReferenceType = VitalSignReferenceType.Referral;
                    await _vitalSignService.SaveVitalSignAsync(facilityReferralDto.VitalSign, true, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
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
            ValidationHelper.IsRequired(errors, nameof(referral.PersonId), referral.PersonId, "PersonId");

            if (!referral.ReferralServiceIds.Any())
                ValidationHelper.AddError(errors, nameof(referral.ReferralServiceIds), "At least one service must be selected.");

            _vitalSignService.ValidateFields(referral.VitalSign, errors, "VitalSign.");

            if (errors.Any())
                throw new ModelValidationException("Validation failed", errors);
        }

    }
}
