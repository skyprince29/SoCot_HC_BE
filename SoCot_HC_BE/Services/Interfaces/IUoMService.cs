using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IUoMService : IRepository<UoM, Guid>
    {
        Task<List<UoM>> GetAllActiveOnlyAsync(CancellationToken cancellationToken = default);
        Task<List<UoM>> GetAllActiveWithCurrentAsync(Guid currentId, CancellationToken cancellationToken = default);
    }
}
