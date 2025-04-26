using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IDepartmentService : IRepository<Department, Guid>
    {
        Task<List<Department>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default);

        Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default);

        Task SaveDepartmentAsync(Department department, CancellationToken cancellationToken = default);

        Task<List<Department>> GetAllActiveOnlyAsync(CancellationToken cancellationToken = default);
        Task<List<Department>> GetAllActiveWithCurrentAsync(Guid currentId, CancellationToken cancellationToken = default);
    }
}
