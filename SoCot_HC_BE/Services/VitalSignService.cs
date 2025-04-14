using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;

namespace SoCot_HC_BE.Services
{
    public class VitalSignService : Repository<VitalSign, Guid>, IVitalSignService
    {
        public VitalSignService(AppDbContext context) : base(context)
        {
        }

        // Get a list of VitalSigns with paging and cancellation support.
        public async Task<List<VitalSign>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();

            // If keyword is provided, filter based on it (example: searching by BloodPressure)
            //if (!string.IsNullOrEmpty(keyword))
            //{
            //    query = query.Where(v => v.BloodPressure.Contains(keyword)); // You can adjust this to your need
            //}

            return await query
                .Skip((pageNo - 1) * limit)
                .Take(limit)
                .ToListAsync(cancellationToken); // Pass the CancellationToken here
        }

        // Count the number of VitalSigns, supporting cancellation.
        public async Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();

            // If keyword is provided, filter based on it (example: searching by BloodPressure)
            //if (!string.IsNullOrEmpty(keyword))
            //{
            //    query = query.Where(v => v.BloodPressure.Contains(keyword)); // You can adjust this to your need
            //}

            return await query.CountAsync(cancellationToken); // Pass the CancellationToken here
        }

        // Optional: Get all VitalSigns without cancellation support (not recommended for production)
        public async Task<List<VitalSign>> GetAllWithoutTokenAsync()
        {
            return await _dbSet.ToListAsync();
        }
    }
}
