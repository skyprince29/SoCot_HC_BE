using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IFacilityService : IRepository<Facility, int>
    {
        // Get a list of Facility with paging, using CancellationToken for async cancellation support.
        Task<List<Facility>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default);

        // Get the total count of Facility, again supporting async cancellation.
        Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default);

        //Save Facility
        Task SaveFacilityAsync(Facility facility, CancellationToken cancellationToken = default);

        Task<List<Facility>> GetAllActiveOnlyAsync(CancellationToken cancellationToken = default);
        Task<List<Facility>> GetAllActiveWithCurrentAsync(int currentId, CancellationToken cancellationToken = default);
    }
}
