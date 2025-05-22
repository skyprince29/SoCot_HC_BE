using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;
using Route = SoCot_HC_BE.Model.Route;

namespace SoCot_HC_BE.Services
{
    public class RouteService : Repository<Route, Guid>, IRouteService
    {
        public RouteService(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Route>> GetAllActiveOnlyAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                 .Where(s => s.IsActive)
                 .ToListAsync(cancellationToken);
        }

        public async Task<List<Route>> GetAllActiveWithCurrentAsync(Guid currentId, CancellationToken cancellationToken = default)
        {
            var activeItems = await _dbSet
                     .Where(s => s.IsActive)
                     .ToListAsync(cancellationToken);

            // Check if the currentId is not among the active items
            bool currentExists = activeItems.Any(s => s.RouteId == currentId);

            if (!currentExists)
            {
                var currentItem = await _dbSet
                    .FirstOrDefaultAsync(s => s.RouteId == currentId, cancellationToken);

                if (currentItem != null)
                    activeItems.Add(currentItem);
            }

            return activeItems;
        }
    }
}
