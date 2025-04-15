using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Services
{
    public class PatientRegistryService : Repository<PatientRegistry, Guid>, IPatientRegistryService
    {
         public PatientRegistryService(AppDbContext context) : base(context)
        {
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
            var query = _dbSet.AsQueryable();

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
            // Determine if new or existing
            bool isNew = patientRegistry.PatientRegistryId == Guid.Empty;
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

            await _context.SaveChangesAsync(cancellationToken);
        }

        private void ValidateFields(PatientRegistry patientRegistry)
        {
            var errors = new Dictionary<string, List<string>>();

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
    }
}
