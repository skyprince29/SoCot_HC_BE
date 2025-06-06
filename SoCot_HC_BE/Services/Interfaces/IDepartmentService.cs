using SCHC_API.Handler;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IDepartmentService : IRepository<Department, Guid>
    {
        Task<PaginationHandler<Department>> GetAllWithPagingAsync(int pageNo, int statusId, List<Guid>? departmentTypes, int limit, string keyword = "", CancellationToken cancellationToken = default);
        Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default);
        Task SaveDepartmentAsync(DepartmentDTO department, CancellationToken cancellationToken = default);
        Task<List<Department>> GetAllActiveOnlyAsync(CancellationToken cancellationToken = default);
        Task<List<Department>> GetAllActiveWithCurrentAsync(Guid currentId, CancellationToken cancellationToken = default);
        Task<List<Department>> GetAllActiveDepartmentByFacility(int facilityId, CancellationToken cancellationToken = default);
        Task<List<Department>> GetDepartmentsByDepartmentTypesAsync(
            int facilityId,
            Guid? currentId,
            List<Guid> departmentTypeIds,
            bool isActiveOnly = true,
            CancellationToken cancellationToken = default);

        Task<PaginationHandler<Department>>  GetDepartmentsExcludedAsync(
          List<Guid>? excludedDepartmentIds, 
          int pageNo, 
          int limit,
          string? keyword,
          CancellationToken cancellationToken = default);
    }
}
