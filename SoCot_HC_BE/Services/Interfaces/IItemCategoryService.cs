using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IItemCategoryService : IRepository<ItemCategory, Guid>
    {
        Task<List<ItemCategory>> GetAllActiveOnlyAsync(CancellationToken cancellationToken = default);
        Task<List<ItemCategory>> GetAllActiveWithCurrentAsync(Guid currentId, CancellationToken cancellationToken = default);
    }
}
