using SoCot_HC_BE.DTO.OldReferralDto;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Model.Requests;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IFamilyMemberService: IRepository<FamilyMember, Guid>
    {
        Task SaveFamilyMember(FamilyMemberRequestDTO familyMemberDto, CancellationToken cancellationToken = default);
        Task<int> CountAsync(string? keyword, CancellationToken cancellationToken = default);
        Task SaveFamilyMembers(List<FamilyMemberRequestDTO> familyMemberDtos, CancellationToken cancellationToken = default);
    }
}
