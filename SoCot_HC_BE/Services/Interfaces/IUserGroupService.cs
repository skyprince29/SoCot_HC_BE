using SCHC_API.Handler;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IUserGroupService : IRepository<UserGroup, int>
    {
        Task<PaginationHandler<UserGroup>> GetAllWithPagingAsync(int pageNo, int statusId, int limit, string keyword = "", CancellationToken cancellationToken = default);
        Task<List<UserGroup>> GetAllActiveOnlyAsync(CancellationToken cancellationToken = default);

    }
}
