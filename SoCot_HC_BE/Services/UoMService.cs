﻿using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;

namespace SoCot_HC_BE.Services
{
    public class UoMService : Repository<UoM, Guid>, IUoMService
    {
        public UoMService(AppDbContext context) : base(context)
        {
        }

        public async Task<List<UoM>> GetAllActiveOnlyAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                 .Where(s => s.IsActive)
                 .ToListAsync(cancellationToken);
        }

        public async Task<List<UoM>> GetAllActiveWithCurrentAsync(Guid currentId, CancellationToken cancellationToken = default)
        {
            var activeItems = await _dbSet
                     .Where(s => s.IsActive)
                     .ToListAsync(cancellationToken);

            // Check if the currentId is not among the active items
            bool currentExists = activeItems.Any(s => s.UoMId == currentId);

            if (!currentExists)
            {
                var currentItem = await _dbSet
                    .FirstOrDefaultAsync(s => s.UoMId == currentId, cancellationToken);

                if (currentItem != null)
                    activeItems.Add(currentItem);
            }

            return activeItems;
        }
    }
}
