using SoCot_HC_BE.DTO.OldReferralDto;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IFamilyService: IRepository<Family, Guid>
    {
        Task SaveFamily(FamilyDto family, CancellationToken cancellationToken);
    }
}
