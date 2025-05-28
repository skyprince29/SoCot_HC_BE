using SCHC_API.Handler;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface INonCommunicableDiseaseService : IRepository<NonCommunicableDisease, Guid>
    {
        Task<NonCommunicableDisease> getNCDAsync(Guid NCDId, CancellationToken cancellationToken);
        Task<PaginationHandler<NonCommunicableDisease>> GetAllWithPagingAsync(int pageNo, int limit, string keyword = "", CancellationToken cancellationToken = default);
        Task SaveOrUpdateDentalRecordAsync(NonCommunicableDiseaseDto NDCDto, CancellationToken cancellationToken = default);

    }
}                                                                