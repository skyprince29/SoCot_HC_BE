using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IServiceCategoryService : IRepository<ServiceCategory, Guid>
    {
        Task<List<ServiceCategory>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default);

        Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default);

        Task SaveServiceCategoryAsync(ServiceCategory serviceCategory, CancellationToken cancellationToken = default);

        Task<List<ServiceCategory>> GetAllActiveOnlyAsync(int? facilityId, CancellationToken cancellationToken = default);
        Task<List<ServiceCategory>> GetAllActiveWithCurrentAsync(int? facilityId, Guid currentId, CancellationToken cancellationToken = default);
        Task<List<ServiceCategory>> GetAllByFacilityAsync(int facilityId, CancellationToken cancellationToken = default);
    }
}
