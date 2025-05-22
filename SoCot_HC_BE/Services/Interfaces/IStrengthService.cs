using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IStrengthService : IRepository<Strength, Guid>
    {
        Task<List<Strength>> GetAllActiveOnlyAsync(CancellationToken cancellationToken = default);
        Task<List<Strength>> GetAllActiveWithCurrentAsync(Guid currentId, CancellationToken cancellationToken = default);
    }
}
