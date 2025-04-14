using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IServiceClassificationService : IRepository<ServiceClassification, int>
    {
        Task<List<ServiceClassification>> GetAllActiveOnlyAsync(CancellationToken cancellationToken = default);
        Task<List<ServiceClassification>> GetAllActiveWithCurrentAsync(int currentId, CancellationToken cancellationToken = default);
    }
}
