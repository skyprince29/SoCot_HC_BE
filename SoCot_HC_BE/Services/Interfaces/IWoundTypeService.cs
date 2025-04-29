using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IWoundTypeService : IRepository<WoundType, int>
    {
        Task<List<WoundType>> GetAllActiveOnlyAsync(CancellationToken cancellationToken = default);
        Task<List<WoundType>> GetAllActiveWithCurrentAsync(int currentId, CancellationToken cancellationToken = default);
    }
}
