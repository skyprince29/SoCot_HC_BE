using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Model.Enums;
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

        public VitalSignDto MapToDto(VitalSign vitalSign, VitalSignReference? reference = null)
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


        public VitalSign DTOToModel(VitalSignDto dto)
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

        public async Task SaveVitalSignAsync(VitalSignDto vitalSignDto,  CancellationToken cancellationToken = default)
        {
            await SaveVitalSignAsync(vitalSignDto, false, cancellationToken);
        }

        public async Task SaveVitalSignAsync(VitalSignDto vitalSignDto, bool isReferrencesaving, CancellationToken cancellationToken = default)
        {
            if (!isReferrencesaving)
            {
                ValidateFields(vitalSignDto);
            }

            VitalSign vitalSign = DTOToModel(vitalSignDto);
            // Determine if new or existing
            bool isNew = vitalSign.VitalSignId == Guid.Empty;
            var referenceSet = _context.Set<VitalSignReference>();
            if (isNew)
            {
                vitalSign.VitalSignId = Guid.NewGuid();
                await _dbSet.AddAsync(vitalSign, cancellationToken);

                // Save VitalSignReference
                var reference = CreateVitalSignReferenceFromDto(vitalSignDto, vitalSign.VitalSignId);
                if (reference != null)
                {
                    await referenceSet.AddAsync(reference, cancellationToken);
                }
            }
            else
            {
                var existing = await _dbSet.FindAsync(new object[] { vitalSign.VitalSignId }, cancellationToken);
                if (existing == null)
                    throw new Exception("Vital Sign not found.");

                // Replace all fields
                _context.Entry(existing).CurrentValues.SetValues(vitalSign);
                _dbSet.Update(existing);

                // Update or Add VitalSignReference
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
                }
                else if (existingReference != null)
                {
                    // If now null but previously had one, delete it
                    referenceSet.Remove(existingReference);
                }
                if (!isReferrencesaving)
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
        }

        public void ValidateFields(VitalSignDto vitalSignDto)
        {
            ValidateFields(vitalSignDto, null, "");
        }

        public void ValidateFields(VitalSignDto vitalSignDto, Dictionary<string, List<string>>? preErrors, string prefix = "")
        {
            var errors = new Dictionary<string, List<string>>(); ;
            bool isStandAlone = (string.IsNullOrEmpty(prefix) && preErrors == null);
            // Reference validation: if one is provided, both must be provided
            if ((isStandAlone) && vitalSignDto.ReferenceId.HasValue ^ vitalSignDto.VitalSignReferenceType.HasValue)
            {
                ValidationHelper.AddError(errors, nameof(vitalSignDto.ReferenceId), "Both ReferenceId and ReferenceType must be provided together.");
                ValidationHelper.AddError(errors, nameof(vitalSignDto.VitalSignReferenceType), "Both ReferenceId and ReferenceType must be provided together.");
            }

            ValidationHelper.IsRequired(errors, nameof(vitalSignDto.Systolic), vitalSignDto.Systolic, "Systolic");
            ValidationHelper.IsRequired(errors, nameof(vitalSignDto.Diastolic), vitalSignDto.Diastolic, "Diastolic");
            Decimal? temperature = vitalSignDto.Temperature;
            if (temperature != null && temperature.Value <= 0)
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
            {
                if (!isStandAlone)
                {
                    // Merge errors into preErrors with prefixed keys
                    foreach (var kvp in errors)
                    {
                        string prefixedKey = prefix + kvp.Key;

                        if (!preErrors.ContainsKey(prefixedKey))
                            preErrors[prefixedKey] = new List<string>();

                        preErrors[prefixedKey].AddRange(kvp.Value);
                    }
                }
                else
                {
                    // No external error collector provided — throw directly
                    throw new ModelValidationException("Validation failed", errors);
                }
            }
        }
    }
}
