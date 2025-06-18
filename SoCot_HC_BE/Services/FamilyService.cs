using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.DTO.OldReferralDto;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Services
{
    public class FamilyService : Repository<Family, Guid>, IFamilyService
    {
        public FamilyService(AppDbContext context) : base(context)
        {
        }

        public async Task SaveFamily(FamilyDto familyDTO, CancellationToken cancellationToken)
        {
            ValidateFields(familyDTO);
            bool isNew = familyDTO.Id == Guid.Empty;
            if (isNew)
            {
                var family = new Family
                {
                    FamilyId = (Guid)familyDTO.Id,
                    HouseholdId = familyDTO.HouseHoldId,
                    PersonId = familyDTO.PersonHeadId,
                    IsActive = true,
                    FamilyNo = GenerateFamilyNo()
                };
            } else
            {
                var existingFamily = await _dbSet.FirstOrDefaultAsync(f => f.FamilyId == familyDTO.Id);

                if (existingFamily == null)
                    throw new Exception("Family not found.");

                _context.Entry(existingFamily).CurrentValues.SetValues(familyDTO);
                await UpdateAsync(existingFamily, cancellationToken);
            }

        }

        private void ValidateFields(FamilyDto familyDto)
        {
            var errors = new Dictionary<string, List<string>>();
            ValidationHelper.IsRequired(errors, nameof(familyDto.HouseHoldId), familyDto.HouseHoldId ,"Household is required");
            if (errors.Any())
                throw new ModelValidationException("Validation failed", errors);
        }

        private string GenerateFamilyNo()
        {
            return $"FAM-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
        }
    }
}
