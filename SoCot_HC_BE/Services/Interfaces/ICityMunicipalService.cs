using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface ICityMunicipalService : IRepository<Municipality, int>
    {
        Task<List<Municipality>> GetMunicipalities(int? ProvinceId, CancellationToken cancellationToken  = default);
    }
}
