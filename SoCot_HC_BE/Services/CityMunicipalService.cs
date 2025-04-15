using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;

namespace SoCot_HC_BE.Services
{
    public class CityMunicipalService : Repository<Municipality, int>, ICityMunicipalService
    {
        public CityMunicipalService(AppDbContext context) : base(context)
        {
        }
        public async Task<List<Municipality>> GetMunicipalities(int? ProvinceId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                          .Where(p => !ProvinceId.HasValue || p.ProvinceId == ProvinceId)
                          .AsNoTracking()
                          .ToListAsync(cancellationToken);
        }
    }
}
