using SCHC_API.Handler;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IUserDepartmentService : IRepository<UserDepartment, Guid>
    {
        Task<PaginationHandler<UserDepartmentDto>> GetAllWithPagingAsync(Guid personId, int pageNo, int limit, string? keyword = null, Boolean? isActive = true, CancellationToken cancellationToken = default);

        Task SaveUserDepartmentAsync(UserDeptModelDto userDeptModelDto, CancellationToken cancellationToken = default);
        Task DeactivateOrActivateUserDepartmentAsync(UserDeptModelDto userDeptModelDto, CancellationToken cancellationToken = default);
        Task<List<Department>> GetDepartmentsByUser(Guid personId, CancellationToken cancellationToken = default);

        Task<PaginationHandler<UserDepartmentAssignedDto>> GetAllWithPagingUserOnUserDepartmentAsync(
         int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default);

    }
}
