using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IBarangayService : IRepository<Barangay, int>
    {
        Task<List<Barangay>> GetBarangays(int? CityMunicipalId, CancellationToken cancellationToken = default);
    }
}
