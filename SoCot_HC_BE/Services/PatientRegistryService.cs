using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Model.Enums;
using SoCot_HC_BE.Persons.Interfaces;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Services
{
    public class PatientRegistryService : Repository<PatientRegistry, Guid>, IPatientRegistryService
    {
        private readonly IPersonService _personService;
         public PatientRegistryService(AppDbContext context, IPersonService personService) : base(context)
        {
             _personService = personService;
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

        public async Task SavePatientRegistryAsync(PatientRegistry patientRegistry, CancellationToken cancellationToken = default)
        {
           await SavePatientRegistryAsync(patientRegistry, cancellationToken);
        }

        public async Task SavePatientRegistryAsync(PatientRegistry patientRegistry, bool isWithValidation = true, CancellationToken cancellationToken = default)
        {
            // Determine if new or existing
            bool isNew = patientRegistry.PatientRegistryId == Guid.Empty;
            if(isWithValidation)
                ValidateFields(patientRegistry);

            if (isNew)
            {
                patientRegistry.PatientRegistryId = Guid.NewGuid();
                string timestamp = DateTime.Now.ToString("yyMMdd-HHmmss");
                //TODO: Update code
                patientRegistry.PatientRegistryCode = $"001-{timestamp}";
                await AddAsync(patientRegistry, cancellationToken);
            }
            else
            {
                var existing = await _dbSet.FindAsync(new object[] { patientRegistry.PatientRegistryId }, cancellationToken);
                if (existing == null)
                    throw new Exception("Patient Registry not found.");

                // Replace all fields
                _context.Entry(existing).CurrentValues.SetValues(patientRegistry);

                await UpdateAsync(existing, cancellationToken);
            }
        }

        private void ValidateFields(PatientRegistry patientRegistry)
        {
            var errors = new Dictionary<string, List<string>>();

            ValidationHelper.IsRequired(errors, nameof(patientRegistry.FacilityId), patientRegistry.FacilityId, "Facility");
            ValidationHelper.IsRequired(errors, nameof(patientRegistry.PatientRegistryType), patientRegistry.PatientRegistryType, "Registry Type");

            bool isTemporary = patientRegistry.IsTemporaryPatient;
            if (!isTemporary)
                ValidationHelper.IsRequired(errors, nameof(patientRegistry.PatientId), patientRegistry.PatientId, "Patient");

            ValidationHelper.IsRequired(errors, nameof(patientRegistry.Name), patientRegistry.Name, "Name");
            ValidationHelper.IsRequired(errors, nameof(patientRegistry.Gender), patientRegistry.Gender, "Gender");


            bool duplicate = _dbSet.Any(p =>
                p.Name == patientRegistry.Name &&
                p.PatientRegistryId != patientRegistry.PatientRegistryId);

            if (duplicate)
                ValidationHelper.AddError(errors, "", "A patient with the same name and birth date already exists.");

            if (errors.Any())
                throw new ModelValidationException("Validation failed", errors);
        }

        public async Task<PatientRegistry> CreatePatientRegistryAsync(string? referralNo, Guid patientId, PatientRegistryType patientRegistryType, int facilityId, bool isUrgent = false, CancellationToken cancellationToken  = default)
        {
            // Fetch person details using the method from PersonService
            var personDetails = await _personService.GetPersonDetailsAsync(patientId, cancellationToken);

            if (personDetails == null)
            {
                throw new Exception("Person not found."); // Handle the case where the person does not exist
            }

            // Create a new PatientRegistry object
            var newPatientRegistry = new PatientRegistry
            {
                PatientRegistryId = Guid.NewGuid(),
                PatientRegistryCode = GeneratePatientRegistryCode(),
                ReferralNo = referralNo,
                PatientId = patientId,
                Name = personDetails.FullName, // Use the fetched full name
                Gender = personDetails.Gender ?? "Unknown", // Use the fetched gender or default to "Unknown"
                Age = personDetails.Age, // Use the fetched age
                Address = personDetails.FullAddress, // Use the fetched full address
                ContactNumber = personDetails.ContactNumber, // Use the fetched contact number
                PatientRegistryType = patientRegistryType,
                FacilityId = facilityId,
                IsTemporaryPatient = false,
                IsUrgent = isUrgent
            };

            // Save the new patient registry asynchronously
            await SavePatientRegistryAsync(newPatientRegistry, false, cancellationToken);

            return newPatientRegistry;
        }

         private string GeneratePatientRegistryCode()
        {
            // Logic to generate a unique PatientRegistryCode
            return $"PR-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
}
