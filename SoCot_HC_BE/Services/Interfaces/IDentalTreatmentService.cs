using SCHC_API.Handler;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IDentalTreatmentService : IRepository<DentalTreatment, Guid>
    {
        Task<PaginationHandler<DentalTreatment>> GetAllWithPagingAsync(int pageNo, int limit, string keyword = "", CancellationToken cancellationToken = default);

        //    Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default);

        //    Task SaveDepartmentAsync(DepartmentDTO department, CancellationToken cancellationToken = default);

        //    Task<List<Department>> GetAllActiveOnlyAsync(CancellationToken cancellationToken = default);
        //    Task<List<Department>> GetAllActiveWithCurrentAsync(Guid currentId, CancellationToken cancellationToken = default);
    }
}
