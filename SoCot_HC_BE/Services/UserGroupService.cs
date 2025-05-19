using Microsoft.EntityFrameworkCore;
using SCHC_API.Handler;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;

namespace SoCot_HC_BE.Services
{
    public class UserGroupService : Repository<UserGroup, int>, IUserGroupService
    {
        public UserGroupService(AppDbContext context) : base(context)
        {
        }

        public async Task<List<UserGroup>> GetAllActiveOnlyAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(x => x.IsActive)
                .ToListAsync();
        }

        public async Task<PaginationHandler<UserGroup>> GetAllWithPagingAsync(int pageNo, int statusId, int limit, string keyword = "", CancellationToken cancellationToken = default)
        {
            int totalRecords = await _dbSet.CountAsync(d =>
                         (statusId == 0 || (statusId == 1 && d.IsActive) || (statusId == 2 && !d.IsActive)) &&
                         (string.IsNullOrEmpty(keyword) || d.UserGroupName.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    );

            var userGroups = await _dbSet
                     .Where(d =>
                         (statusId == 0 || (statusId == 1 && d.IsActive) || (statusId == 2 && !d.IsActive)) &&
                         (string.IsNullOrEmpty(keyword) || d.UserGroupName.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                     )
                     .AsNoTracking()
                     .ToListAsync();

            var paginatedResult = new PaginationHandler<UserGroup>(userGroups, totalRecords, pageNo, limit);
            return paginatedResult;
        }
    }
}
