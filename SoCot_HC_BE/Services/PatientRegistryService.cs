using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Dtos;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Persons.Interfaces;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Services
{
    public class PatientRegistryService : Repository<PatientRegistry, Guid>, IPatientRegistryService
    {
        private readonly IPersonService _personService;
        private readonly ITransactionFlowHistoryService _transactionFlowHistoryService;
        public PatientRegistryService
            (AppDbContext context,
            IPersonService personService,
            ITransactionFlowHistoryService transactionFlowHistoryService
            ) : base(context)
        {
            _personService = personService;
            _transactionFlowHistoryService = transactionFlowHistoryService;
        }

        //Overloads method from Repository, Added facility to eager loading
        public override async Task<PatientRegistry?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(pr => pr.Facility)
                .FirstOrDefaultAsync(pr => pr.PatientRegistryId == id, cancellationToken);
        }

        public async Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query
                        .Where(v => v.Name.Contains(keyword)
                             || (v.Address != null && v.Address.Contains(keyword)));
            }
            return await query.CountAsync(cancellationToken); // Pass the CancellationToken here
        }

        public async Task<List<PatientRegistry>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.Include(p => p.Facility).AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query
                        .Where(v => v.Name.Contains(keyword)
                            || (v.Address != null && v.Address.Contains(keyword)));
            }

            return await query
                .Skip((pageNo - 1) * limit)
                .Take(limit)
                .ToListAsync(cancellationToken); // Pass the CancellationToken here
        }

        private PatientRegistry DTOToModel(PatientRegistryDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            var patientId = dto.PatientId;
            var patientRegistryId = dto.PatientRegistryId;
            var statusId = dto.StatusId;
            return new PatientRegistry
            {
                PatientRegistryId = patientRegistryId == Guid.Empty ? Guid.Empty : patientRegistryId,
                PatientRegistryCode = dto.PatientRegistryCode ?? string.Empty, // fallback if null
                ReferralNo = dto.ReferralNo,
                PatientId = ((!patientId.HasValue || patientId == Guid.Empty) ? null : patientId),
                Name = dto.Name ?? string.Empty,
                Address = dto.Address,
                Gender = dto.Gender ?? string.Empty,
                ContactNumber = dto.ContactNumber,
                Age = dto.Age,
                IsTemporaryPatient = (!patientId.HasValue || patientId == Guid.Empty),
                IsUrgent = dto.IsUrgent,
                PatientRegistryType = dto.PatientRegistryType,
                FacilityId = dto.FacilityId,
                IsActive = dto.IsActive,
                StatusId = statusId.HasValue ? statusId.Value : (byte)0,
                ServiceId = dto.ServiceId,
                CreatedBy = dto.CreatedBy,
                CreatedDate = dto.CreatedDate
            };
        }


        public async Task<PatientRegistry> SavePatientRegistryAsync(PatientRegistryDto patientRegistry, CancellationToken cancellationToken = default)
        {
           return await SavePatientRegistryAsync(patientRegistry, true, cancellationToken);
        }

        public async Task<PatientRegistry> SavePatientRegistryAsync(PatientRegistryDto patientRegistryDto, bool isWithValidation, CancellationToken cancellationToken = default)
        {
            // Use a transaction to ensure consistency
            using (var transaction = _context.Database.BeginTransaction())
            {
                if (isWithValidation)
                    ValidateFields(patientRegistryDto);
                try
                {
                    PatientRegistry patientRegistry = DTOToModel(patientRegistryDto);
                    // Determine if new or existing
                    bool isNew = patientRegistry.PatientRegistryId == Guid.Empty;
                    if (isNew)
                    {
                        // Create a new PatientRegistry
                        patientRegistry.PatientRegistryId = Guid.NewGuid();
                        string timestamp = DateTime.Now.ToString("yyMMdd-HHmmss");
                        patientRegistry.PatientRegistryCode = $"001-{timestamp}";

                        await _transactionFlowHistoryService.StarterLogAsync(patientRegistry, cancellationToken);
                        await AddAsync(patientRegistry, cancellationToken);
                    }
                    else
                    {
                        var existing = await _dbSet.FindAsync(new object[] { patientRegistry.PatientRegistryId }, cancellationToken);
                        if (existing == null)
                            throw new Exception("Patient Registry not found.");

                        // Replace all fields
                        _context.Entry(existing).CurrentValues.SetValues(patientRegistry);

                        // Save once
                        await _context.SaveChangesAsync(cancellationToken);
                    }

                    // Commit the transaction
                    transaction.Commit();

                    // Return the saved entity with its ID
                    return patientRegistry;
                }
                catch (Exception ex)
                {
                    // Rollback if any error occurs
                    transaction.Rollback();
                    throw new Exception("Failed to save Patient Registry and log: " + ex.Message, ex);
                }
            }
        }

        private void ValidateFields(PatientRegistryDto patientRegistry)
        {
            var errors = new Dictionary<string, List<string>>();

            ValidationHelper.IsRequired(errors, nameof(patientRegistry.FacilityId), patientRegistry.FacilityId, "Facility");
            ValidationHelper.IsRequired(errors, nameof(patientRegistry.PatientRegistryType), patientRegistry.PatientRegistryType, "Registry Type");

            bool isTemporary = patientRegistry.IsTemporaryPatient;
            if (!isTemporary)
                ValidationHelper.IsRequired(errors, nameof(patientRegistry.PatientId), patientRegistry.PatientId, "Patient");

            ValidationHelper.IsRequired(errors, nameof(patientRegistry.Name), patientRegistry.Name, "Name");
            ValidationHelper.IsRequired(errors, nameof(patientRegistry.Gender), patientRegistry.Gender, "Gender");
            ValidationHelper.IsRequired(errors, nameof(patientRegistry.ServiceId), patientRegistry.ServiceId, "Service");

            // Need to discuss further for disabling duplicate registry
            //bool duplicate = _dbSet.Any(p =>
            //    p.Name == patientRegistry.Name &&
            //    p.PatientRegistryId != patientRegistry.PatientRegistryId);

            //if (duplicate)
            //    ValidationHelper.AddError(errors, nameof(patientRegistry.Name), "A patient with the same name and birth date already exists.");

            if (errors.Any())
                throw new ModelValidationException("Validation failed", errors);
        }

        public async Task<PatientRegistry> CreatePatientRegistryAsync(AcceptReferralDto acceptReferralDto, CancellationToken cancellationToken  = default)
        {
            Guid patientId = acceptReferralDto.patientId;
            // Fetch person details using the method from PersonService
            var personDetails = await _personService.GetPersonDetailsAsync(patientId, cancellationToken);

            if (personDetails == null)
            {
                throw new Exception("Person not found."); // Handle the case where the person does not exist
            }

            // Create a new PatientRegistry object
            var newPatientRegistry = new PatientRegistryDto
            {
                PatientRegistryId = Guid.NewGuid(),
                PatientRegistryCode = GeneratePatientRegistryCode(),
                ReferralNo = acceptReferralDto.referralNo,
                PatientId = patientId,
                Name = personDetails.FullName, // Use the fetched full name
                Gender = personDetails.Gender ?? "Unknown", // Use the fetched gender or default to "Unknown"
                Age = personDetails.Age, // Use the fetched age
                Address = personDetails.FullAddress, // Use the fetched full address
                ContactNumber = personDetails.ContactNumber, // Use the fetched contact number
                PatientRegistryType = acceptReferralDto.patientRegistryType,
                FacilityId = acceptReferralDto.facilityId,
                IsTemporaryPatient = false,
                IsUrgent = acceptReferralDto.isUrgent,
                ServiceId = acceptReferralDto.serviceId
            };

            // 1. Call the Save method and CAPTURE the returned entity
            var savedRegistry = await SavePatientRegistryAsync(newPatientRegistry, false, cancellationToken);

            // 2. RETURN the saved entity
            return savedRegistry;
        }

         private string GeneratePatientRegistryCode()
        {
            // Logic to generate a unique PatientRegistryCode
            return $"PR-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
}
