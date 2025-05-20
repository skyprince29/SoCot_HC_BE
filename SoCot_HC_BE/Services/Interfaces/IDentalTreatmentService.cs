using Microsoft.AspNetCore.Mvc;
using SCHC_API.Handler;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IDentalTreatmentService : IRepository<DentalTreatment, Guid>
    {
        Task<PaginationHandler<DentalTreatment>> GetAllWithPagingAsync(int pageNo, int limit, string keyword = "", CancellationToken cancellationToken = default);
        Task AcceptDentalTreatment(Guid dentalTreatmentId, CancellationToken cancellationToken = default);
    }
}
