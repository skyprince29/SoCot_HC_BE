using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IWRAService : IRepository<WRA, Guid>
    {
        // Get the total count of Designation, again supporting async cancellation.
        Task<List<WRA>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default);
        Task SaveWRAAsync(WRADto wra, CancellationToken cancellationToken = default);
        Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default);
    }
}

