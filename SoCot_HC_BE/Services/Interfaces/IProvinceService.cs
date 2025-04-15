using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IProvinceService : IRepository<Province, int>
    {
        Task<List<Province>> GetProvinces(CancellationToken cancellationToken = default);
    }
}
