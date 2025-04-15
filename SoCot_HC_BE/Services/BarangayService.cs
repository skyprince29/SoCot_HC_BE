using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;

namespace SoCot_HC_BE.Services
{
    public class BarangayService : Repository<Barangay, int>, IBarangayService
    {
        public BarangayService(AppDbContext context) : base(context)
        {
        }
        public async Task<List<Barangay>> GetBarangays(int? CityMunicipalId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                        .Where(b => !CityMunicipalId.HasValue || b.MunicipalityId == CityMunicipalId)        
                        .AsNoTracking().ToListAsync();
        }
    }
}
