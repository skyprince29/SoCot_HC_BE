using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;

namespace SoCot_HC_BE.Services
{
    public class FacilityService : Repository<Facility, int>, IFacilityService
    {
        public FacilityService(AppDbContext context) : base(context)
        {
        }

        // Get a list of Facility with paging and cancellation support.
        public async Task<List<Facility>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(v => v.FacilityName.Contains(keyword)); // You can adjust this to your need
            }

            return await query
                .Skip((pageNo - 1) * limit)
                .Take(limit)
                .ToListAsync(cancellationToken); // Pass the CancellationToken here
        }

        public async Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query
                        .Where(v => v.FacilityName.Contains(keyword));
            }
            return await query.CountAsync(cancellationToken); // Pass the CancellationToken here
        }

        public async Task<List<Facility>> GetAllActiveOnlyAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(s => s.IsActive)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Facility>> GetAllActiveWithCurrentAsync(int currentId, CancellationToken cancellationToken = default)
        {
            var activeItems = await _dbSet
                     .Where(s => s.IsActive)
                     .ToListAsync(cancellationToken);

            // Check if the currentId is not among the active items
            bool currentExists = activeItems.Any(s => s.FacilityId == currentId);

            if (!currentExists)
            {
                var currentItem = await _dbSet
                    .FirstOrDefaultAsync(s => s.FacilityId == currentId, cancellationToken);

                if (currentItem != null)
                    activeItems.Add(currentItem);
            }

            return activeItems;
        }

        public async Task SaveFacilityAsync(Facility facility, CancellationToken cancellationToken = default)
        {
            // Determine if new or existing
            bool isNew = facility.FacilityId == 0;
            //ValidateFields(patientRegistry);


            if (isNew)
            {
                string timestamp = DateTime.Now.ToString("yyMMdd-HHmmss");
                //TODO: Update code
                facility.FacilityCode = $"001-{timestamp}";
                await AddAsync(facility, cancellationToken);
            }
            else
            {
                var existing = await _dbSet.FindAsync(new object[] { facility.FacilityId }, cancellationToken);
                if (existing == null)
                    throw new Exception("Facility not found.");

                // Replace all fields
                _context.Entry(existing).CurrentValues.SetValues(facility);

                await UpdateAsync(existing, cancellationToken);
            }
        }
    }
}
