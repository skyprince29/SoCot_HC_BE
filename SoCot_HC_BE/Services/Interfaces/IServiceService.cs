using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IServiceService : IRepository<Service, Guid>
    {
        // Get a list of Services with paging, using CancellationToken for async cancellation support.
        Task<List<Service>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default);

        // Get the total count of Services, again supporting async cancellation.
        Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default);

        // Save Service
        Task SaveServiceAsync(Service service, CancellationToken cancellationToken = default);
    }
} 