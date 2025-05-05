using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Designations.Interfaces
{
    public interface IDesignationService : IRepository<Designation, Guid>
    {
        // Get a list of Designation with paging, using CancellationToken for async cancellation support.
        Task<List<Designation>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default);

        // Get the total count of Designation, again supporting async cancellation.
        Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default);

        // Save Designation
        Task SaveDesignationAsync(Designation designation, CancellationToken cancellationToken = default);
        Task<List<Designation>> GetAllActiveOnlyAsync(CancellationToken cancellationToken = default);
        Task<List<Designation>> GetAllActiveWithCurrentAsync(Guid currentId, CancellationToken cancellationToken = default);
    }
}