using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Personnels.Interfaces
{
    public interface IPersonnelService : IRepository<Personnel, Guid>
    {
        // Get a list of Personnel with paging, using CancellationToken for async cancellation support.
        Task<List<Personnel>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default);

        // Get the total count of Personnel, again supporting async cancellation.
        Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default);

        // Save Personnel
        Task SavePersonnelAsync(Personnel personnel, CancellationToken cancellationToken = default);
    }
}