using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;

namespace SoCot_HC_BE.Services
{
    public class WoundTypeService : Repository<WoundType, int>, IWoundTypeService
    {
        public WoundTypeService(AppDbContext context) : base(context)
        {
        }

        public async Task<List<WoundType>> GetAllActiveOnlyAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(s => s.IsActive)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<WoundType>> GetAllActiveWithCurrentAsync(int currentId, CancellationToken cancellationToken = default)
        {
            var activeItems = await _dbSet
                    .Where(s => s.IsActive)
                    .ToListAsync(cancellationToken);

            // Check if the currentId is not among the active items
            bool currentExists = activeItems.Any(s => s.WoundTypeId == currentId);

            if (!currentExists)
            {
                var currentItem = await _dbSet
                    .FirstOrDefaultAsync(s => s.WoundTypeId == currentId, cancellationToken);

                if (currentItem != null)
                    activeItems.Add(currentItem);
            }

            return activeItems;
        }

        // Add any additional methods specific to ServiceClassification if needed
    }
}
