using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IDepartmentTypeService : IRepository<DepartmentType, Guid>
    {
        Task<List<DepartmentType>> GetAllActiveOnlyAsync(CancellationToken cancellationToken = default);
        Task<List<DepartmentType>> GetAllActiveWithCurrentAsync(Guid currentId, CancellationToken cancellationToken = default);
    }
}
