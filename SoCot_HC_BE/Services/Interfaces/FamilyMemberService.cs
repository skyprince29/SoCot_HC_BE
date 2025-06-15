using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.DTO.OldReferralDto;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Model.Requests;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Utils;
using System;
using System.Threading;

namespace SoCot_HC_BE.Services.Interfaces
{
    public class FamilyMemberService : Repository<FamilyMember, Guid>, IFamilyMemberService
    {
        public FamilyMemberService(AppDbContext context) : base(context)
        {
        }

        public async Task SaveFamilyMembers(List<FamilyMemberRequestDTO> familyMemberDtos, CancellationToken cancellationToken = default)
        {
            if (familyMemberDtos == null && familyMemberDtos.Count == 0)
            {
                throw new Exception("Family member/s is required");
            }
            foreach (var familyMemberDto in familyMemberDtos)
            {
                await SaveFamilyMember(familyMemberDto, cancellationToken);
            }
        }

        public async Task<int> CountAsync(string? keyword, CancellationToken cancellationToken)
        {
            return await _dbSet.CountAsync(cancellationToken);
        }

        public async Task SaveFamilyMember(FamilyMemberRequestDTO familyMemberDto, CancellationToken cancellationToken)
        {
            bool isNew = familyMemberDto.FamilyMemberId == Guid.Empty;
            ValidateFields(familyMemberDto);

            FamilyMember familyMember = new FamilyMember()
            {
                FamilyMemberId = Guid.NewGuid(),
                PersonId = familyMemberDto.PersonId,
                FamilyId = familyMemberDto.FamilyId
            };
            if (isNew) {
                await AddAsync(familyMember);
            } else
            {
                var existing = await _dbSet
                 .FirstOrDefaultAsync(p => p.FamilyMemberId == familyMemberDto.FamilyMemberId, cancellationToken);

                if (existing == null)
                    throw new Exception("Person not found.");

                _context.Entry(existing).CurrentValues.SetValues(familyMember);
                await UpdateAsync(existing, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        private void ValidateFields(FamilyMemberRequestDTO familyMemberDto)
        {
            var errors = new Dictionary<string, List<string>>();

            ValidationHelper.IsRequired(errors, nameof(familyMemberDto.FamilyId), familyMemberDto.FamilyId, "Family");
            ValidationHelper.IsRequired(errors, nameof(familyMemberDto.PersonId), familyMemberDto.PersonId, "Person");

            if (errors.Any())
                throw new ModelValidationException("Validation failed", errors);
        }
    }
}
