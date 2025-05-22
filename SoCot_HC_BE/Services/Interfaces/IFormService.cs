using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IFormService : IRepository<Form, Guid>
    {
        Task<List<Form>> GetAllActiveOnlyAsync(CancellationToken cancellationToken = default);
        Task<List<Form>> GetAllActiveWithCurrentAsync(Guid currentId, CancellationToken cancellationToken = default);
    }
}
