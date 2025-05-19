using SCHC_API.Handler;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IUserAccountService : IRepository<UserAccount, Guid>
    {
        Task<PaginationHandler<UserAccount>> GetAllWithPagingAsync(int pageNo, int statusId, int facilityId, int userGroupId, int limit, string keyword = "", CancellationToken cancellationToken = default);
        Task SaveUserAcccountAsync(UserAccountDTO userAccount, CancellationToken cancellationToken = default);
    }
}
