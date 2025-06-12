using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Model.Enums;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Services
{
    public class VitalSignService : Repository<VitalSign, Guid>, IVitalSignService
    {
        public VitalSignService(AppDbContext context) : base(context)
        {
        }

        private VitalSignDto MapToDto(VitalSign vitalSign, VitalSignReference? reference = null)
        {
            return new VitalSignDto
            {
                VitalSignId = vitalSign.VitalSignId,
                ReferenceId = reference?.ReferenceId,
                VitalSignReferenceType = reference?.VitalSignReferenceType,

                Temperature = vitalSign.Temperature,
                Height = vitalSign.Height,
                Weight = vitalSign.Weight,
                RespiratoryRate = vitalSign.RespiratoryRate,
                CardiacRate = vitalSign.CardiacRate,
                Systolic = vitalSign.Systolic,
                Diastolic = vitalSign.Diastolic,
                BloodPressure = vitalSign.BloodPressure,

                CreatedBy = vitalSign.CreatedBy,
                CreatedDate = vitalSign.CreatedDate
            };
        }

        public async Task<VitalSignDto?> GetVitalSignDtoAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var vitalSign = await _dbSet
                .FirstOrDefaultAsync(f => f.VitalSignId == id, cancellationToken);

            if (vitalSign == null)
                return null;

            var reference = await _context.Set<VitalSignReference>()
                .FirstOrDefaultAsync(r => r.VitalSignId == id, cancellationToken);

            return MapToDto(vitalSign, reference);
        }

        public async Task<List<VitalSignDto>> GetAllWithPagingAsync(
            int pageNo,
            int limit,
            VitalSignReferenceType? referenceType = null,
            Guid? referenceId = null,
            CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();

            // Preload paged vital signs
            var pagedVitalSigns = await query
                .OrderByDescending(v => v.CreatedDate)
                .Skip((pageNo - 1) * limit)
                .Take(limit)
                .ToListAsync(cancellationToken);

            var vitalSignIds = pagedVitalSigns.Select(v => v.VitalSignId).ToList();

            // Build the reference query
            var referenceQuery = _context.Set<VitalSignReference>()
                .Where(r => vitalSignIds.Contains(r.VitalSignId));

            if (referenceType.HasValue)
            {
                referenceQuery = referenceQuery.Where(r => r.VitalSignReferenceType == referenceType.Value);
            }

            if (referenceId.HasValue && referenceId.Value != Guid.Empty)
            {
                referenceQuery = referenceQuery.Where(r => r.ReferenceId == referenceId.Value);
            }

            var references = await referenceQuery.ToListAsync(cancellationToken);
            var referenceDict = references.ToDictionary(r => r.VitalSignId, r => r);

            // Filter based on reference presence if filters were applied
            var result = pagedVitalSigns
                .Where(v => !referenceType.HasValue && !referenceId.HasValue || referenceDict.ContainsKey(v.VitalSignId))
                .Select(v => {
                    referenceDict.TryGetValue(v.VitalSignId, out var refItem);
                    return MapToDto(v, refItem);
                })
                .ToList();

            return result;
        }

        public async Task<int> CountAsync(
            VitalSignReferenceType? referenceType = null,
            Guid? referenceId = null,
            CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();

            var allVitalSigns = await query.ToListAsync(cancellationToken);

            if (!referenceType.HasValue && !referenceId.HasValue)
                return allVitalSigns.Count;

            var vitalSignIds = allVitalSigns.Select(v => v.VitalSignId).ToList();

            var referenceQuery = _context.Set<VitalSignReference>()
                .Where(r => vitalSignIds.Contains(r.VitalSignId));

            if (referenceType.HasValue)
            {
                referenceQuery = referenceQuery.Where(r => r.VitalSignReferenceType == referenceType.Value);
            }

            if (referenceId.HasValue && referenceId.Value != Guid.Empty)
            {
                referenceQuery = referenceQuery.Where(r => r.ReferenceId == referenceId.Value);
            }

            var matchedIds = await referenceQuery
                .Select(r => r.VitalSignId)
                .Distinct()
                .ToListAsync(cancellationToken);

            return matchedIds.Count;
        }


        private VitalSign DTOToModel(VitalSignDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            return new VitalSign
            {
                VitalSignId = dto.VitalSignId == Guid.Empty ? Guid.Empty : dto.VitalSignId,
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

        private VitalSignReference CreateVitalSignReferenceFromDto(VitalSignDto dto, Guid vitalSignId)
        {
            if (dto.ReferenceId == null || dto.ReferenceId == Guid.Empty || dto.VitalSignReferenceType == null)
                return null;

            return new VitalSignReference
            {
                VitalSignReferenceId = Guid.NewGuid(),
                VitalSignId = vitalSignId,
                ReferenceId = dto.ReferenceId.Value,
                VitalSignReferenceType = dto.VitalSignReferenceType.Value
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

                // Save VitalSignReference
                var reference = CreateVitalSignReferenceFromDto(vitalSignDto, vitalSign.VitalSignId);
                if (reference != null)
                {
                    await _context.Set<VitalSignReference>().AddAsync(reference, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
            else
            {
                var existing = await _dbSet.FindAsync(new object[] { vitalSign.VitalSignId }, cancellationToken);
                if (existing == null)
                    throw new Exception("Vital Sign not found.");

                // Replace all fields
                _context.Entry(existing).CurrentValues.SetValues(vitalSign);

                await UpdateAsync(existing, cancellationToken);

                // Update or Add VitalSignReference
                var referenceSet = _context.Set<VitalSignReference>();
                var existingReference = await referenceSet
                    .FirstOrDefaultAsync(x => x.VitalSignId == vitalSign.VitalSignId, cancellationToken);

                if (vitalSignDto.ReferenceId != null && vitalSignDto.VitalSignReferenceType != null)
                {
                    if (existingReference != null)
                    {
                        existingReference.ReferenceId = vitalSignDto.ReferenceId.Value;
                        existingReference.VitalSignReferenceType = vitalSignDto.VitalSignReferenceType.Value;
                        _context.Update(existingReference);
                    }
                    else
                    {
                        var newReference = CreateVitalSignReferenceFromDto(vitalSignDto, vitalSign.VitalSignId);
                        await referenceSet.AddAsync(newReference, cancellationToken);
                    }

                    await _context.SaveChangesAsync(cancellationToken);
                }
                else if (existingReference != null)
                {
                    // If now null but previously had one, delete it
                    referenceSet.Remove(existingReference);
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
        }

        private void ValidateFields(VitalSignDto vitalSignDto)
        {
            var errors = new Dictionary<string, List<string>>();
            // Reference validation: if one is provided, both must be provided
            if (vitalSignDto.ReferenceId.HasValue ^ vitalSignDto.VitalSignReferenceType.HasValue)
            {
                ValidationHelper.AddError(errors, nameof(vitalSignDto.ReferenceId), "Both ReferenceId and ReferenceType must be provided together.");
                ValidationHelper.AddError(errors, nameof(vitalSignDto.VitalSignReferenceType), "Both ReferenceId and ReferenceType must be provided together.");
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
