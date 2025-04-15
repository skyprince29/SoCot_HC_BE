using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;

namespace SoCot_HC_BE.Services
{
    public class ProvinceService : Repository<Province, int>, IProvinceService
    {
        public ProvinceService(AppDbContext context) : base(context)
        {
        }

        public async Task<Province> GetProvince(int ProviceId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.(ProviceId);
        }

        public async Task<List<Province>> GetProvinces(CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }
    }
}
