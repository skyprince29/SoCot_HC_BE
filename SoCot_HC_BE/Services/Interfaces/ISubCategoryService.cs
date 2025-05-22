using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface ISubCategoryService : IRepository<SubCategory, Guid>
    {
        Task<List<SubCategory>> GetAllActiveOnlyAsync(CancellationToken cancellationToken = default);
        Task<List<SubCategory>> GetAllActiveWithCurrentAsync(Guid currentId, CancellationToken cancellationToken = default);
    }
}
