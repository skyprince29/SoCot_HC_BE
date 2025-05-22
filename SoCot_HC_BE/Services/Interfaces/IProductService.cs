using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IProductService : IRepository<Product, Guid>
    {
        Task<List<Product>> GetAllActiveOnlyAsync(CancellationToken cancellationToken = default);
        Task<List<Product>> GetAllActiveWithCurrentAsync(Guid currentId, CancellationToken cancellationToken = default);
    }
}
