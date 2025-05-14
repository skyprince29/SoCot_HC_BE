using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Utils;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SoCot_HC_BE.Services
{
    public class VitalSignService : Repository<VitalSign, Guid>, IVitalSignService
    {
        public VitalSignService(AppDbContext context) : base(context)
        {
        }

        // Get a list of VitalSigns with paging and cancellation support.
        public async Task<List<VitalSign>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();

            return await query
                .Skip((pageNo - 1) * limit)
                .Take(limit)
                .ToListAsync(cancellationToken); // Pass the CancellationToken here
        }

        // Count the number of VitalSigns, supporting cancellation.
        public async Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();
            return await query.CountAsync(cancellationToken); // Pass the CancellationToken here
        }

        // Optional: Get all VitalSigns without cancellation support (not recommended for production)
        public async Task<List<VitalSign>> GetAllWithoutTokenAsync()
        {
            return await _dbSet.ToListAsync();
        }

        private VitalSign DTOToModel(VitalSignDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            return new VitalSign
            {
                VitalSignId = dto.VitalSignId == Guid.Empty ? Guid.Empty : dto.VitalSignId,
                PatientRegistryId = (dto.PatientRegistryId.HasValue ?
                    (dto.PatientRegistryId == Guid.Empty ? dto.PatientRegistryId : null) : null),
                Temperature = dto.Temperature,
                Height = dto.Height,
                Weight = dto.Weight,
                RespiratoryRate = dto.RespiratoryRate,
                CardiacRate = dto.CardiacRate,
                Systolic = dto.Systolic,
                Diastolic = dto.Diastolic,
                BloodPressure = dto.Systolic + "/" + dto.Diastolic,

                // Copying base audit properties
                CreatedBy = dto.CreatedBy,
                CreatedDate = dto.CreatedDate,
                UpdatedBy = dto.UpdatedBy,
                UpdatedDate = dto.UpdatedDate
            };
        }

        public async Task SaveVitalSignAsync(VitalSignDto vitalSignDto, CancellationToken cancellationToken = default)
        {
            ValidateFields(vitalSignDto);
            VitalSign vitalSign = DTOToModel(vitalSignDto);
            // Determine if new or existing
            bool isNew = vitalSign.VitalSignId == Guid.Empty;
            if (isNew)
            {
                vitalSign.VitalSignId = Guid.NewGuid();
                await AddAsync(vitalSign, cancellationToken);
            }
            else
            {
                var existing = await _dbSet.FindAsync(new object[] { vitalSign.VitalSignId }, cancellationToken);
                if (existing == null)
                    throw new Exception("Vital Sign not found.");

                // Replace all fields
                _context.Entry(existing).CurrentValues.SetValues(vitalSign);

                await UpdateAsync(existing, cancellationToken);
            }
        }

        private void ValidateFields(VitalSignDto vitalSignDto)
        {
            var errors = new Dictionary<string, List<string>>();
            Guid? patientRegistryId = vitalSignDto.PatientRegistryId;
            if (patientRegistryId.HasValue && patientRegistryId.Value != Guid.Empty)
            {
                var ptExists = _context.PatientRegistry.Any(f => f.PatientRegistryId == patientRegistryId);
                if (ptExists)
                {
                    ValidationHelper.AddError(errors, nameof(vitalSignDto.PatientRegistryId), "Patient registry is invalid.");
                }
            }

            ValidationHelper.IsRequired(errors, nameof(vitalSignDto.Systolic), vitalSignDto.Systolic, "Systolic");
            ValidationHelper.IsRequired(errors, nameof(vitalSignDto.Diastolic), vitalSignDto.Diastolic, "Diastolic");
            Decimal? temperature = vitalSignDto.Temperature;
            if (temperature!= null && temperature.Value <= 0)
            {
                ValidationHelper.AddError(errors, nameof(vitalSignDto.Temperature), "Temperature is invalid value.");
            }
            Decimal? height = vitalSignDto.Height;
            if (height != null && height.Value <= 0)
            {
                ValidationHelper.AddError(errors, nameof(vitalSignDto.Height), "Height is required.");
            }
            Decimal? weight = vitalSignDto.Weight;
            if (height != null && height <= 0)
            {
                ValidationHelper.AddError(errors, nameof(vitalSignDto.Weight), "Weight is required.");
            }
            int? cardiacRate = vitalSignDto.CardiacRate;
            if (cardiacRate != null && cardiacRate.Value <= 0)
            {
                ValidationHelper.AddError(errors, nameof(vitalSignDto.CardiacRate), "Cardiac Rate is invalid value.");
            }
            int? respiratoryRate = vitalSignDto.RespiratoryRate;
            if (respiratoryRate != null && respiratoryRate.Value <= 0)
            {
                ValidationHelper.AddError(errors, nameof(vitalSignDto.RespiratoryRate), "Respiratory Rate is invalid value.");
            }
            if (errors.Any())
                throw new ModelValidationException("Validation failed", errors);
        }
    }
}
