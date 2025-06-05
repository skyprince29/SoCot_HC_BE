using SCHC_API.Handler;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IItemService : IRepository<Item, Guid>
    {
        Task<PaginationHandler<Item>> GetAllWithPagingAsync(int pageNo, int statusId, List<Guid>? itemCategories, int limit, string keyword = "", CancellationToken cancellationToken = default);
        Task SaveItemAsync(ItemDTO item, CancellationToken cancellationToken = default);


    }
}
