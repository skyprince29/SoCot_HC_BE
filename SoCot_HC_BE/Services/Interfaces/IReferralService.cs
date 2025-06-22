using SoCot_HC_BE.DTO.OldReferralDto;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IReferralService : IRepository<ReferralDto, Guid>
    {
        Task<UHCReferralDTO> GetUHCReferralAsync(int referralId , int facilityId, CancellationToken cancellationToken = default);
        Task<UHCReferralDTO> MarkReferralArrived(int referralId, int facilityId, CancellationToken cancellationToken = default);
    }
}
