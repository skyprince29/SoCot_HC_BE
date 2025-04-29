using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Services
{
    public class ServiceCategoryService : Repository<ServiceCategory, Guid>, IServiceCategoryService
    { 
        public ServiceCategoryService(AppDbContext context) : base(context)
        {
        }

        public override async Task<ServiceCategory?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {

            // You can include related data here if needed, like navigation properties
            var department = await _dbSet
                .Include(f => f.Facility)
                .FirstOrDefaultAsync(f => f.ServiceCategoryId == id, cancellationToken);

            return department;
        }

        public async Task<List<ServiceCategory>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet
                 .Include(f => f.Facility)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(v => v.ServiceCategoryName.Contains(keyword)); // You can adjust this to your need
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
                        .Where(v => v.ServiceCategoryName.Contains(keyword));
            }
            return await query.CountAsync(cancellationToken); // Pass the CancellationToken here
        }

        public async Task<List<ServiceCategory>> GetAllActiveOnlyAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(s => s.IsActive)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ServiceCategory>> GetAllActiveWithCurrentAsync(Guid currentId, CancellationToken cancellationToken = default)
        {
            var activeItems = await _dbSet
                     .Where(s => s.IsActive)
                     .ToListAsync(cancellationToken);

            // Check if the currentId is not among the active items
            bool currentExists = activeItems.Any(s => s.ServiceCategoryId == currentId);

            if (!currentExists)
            {
                var currentItem = await _dbSet
                    .FirstOrDefaultAsync(s => s.ServiceCategoryId == currentId, cancellationToken);

                if (currentItem != null)
                    activeItems.Add(currentItem);
            }

            return activeItems;
        }

        public async Task SaveServiceCategoryAsync(ServiceCategory serviceCategory, CancellationToken cancellationToken = default)
        {
            // Determine if new or existing
            bool isNew = serviceCategory.ServiceCategoryId == Guid.Empty;
            ValidateFields(serviceCategory);

            if (isNew)
            {
                string timestamp = DateTime.Now.ToString("yyMMdd-HHmmss");
                await AddAsync(serviceCategory, cancellationToken);
            }
            else
            {
                var existing = await _dbSet.FindAsync(new object[] { serviceCategory.ServiceCategoryId }, cancellationToken);
                if (existing == null)
                    throw new Exception("Service Category not found.");

                // Replace all fields
                _context.Entry(existing).CurrentValues.SetValues(serviceCategory);

                await UpdateAsync(existing, cancellationToken);
            }
        }

        private void ValidateFields(ServiceCategory serviceCategory)
        {
            var errors = new Dictionary<string, List<string>>();


            int facilityId = serviceCategory.FacilityId;
            ValidationHelper.IsRequired(errors, nameof(serviceCategory.FacilityId), facilityId, "Facility");
            // Verify that the FacilityId exists
            var facilityExists = _context.Facility.Any(f => f.FacilityId == facilityId);
            if (!facilityExists && facilityId > 0)
            {
                ValidationHelper.AddError(errors, nameof(serviceCategory.FacilityId), "Facility is invalid.");
            }

            ValidationHelper.IsRequired(errors, nameof(serviceCategory.ServiceCategoryName), serviceCategory.ServiceCategoryName, "Service Category Name");
            bool duplicate = _dbSet.Any(s =>
                s.ServiceCategoryName == serviceCategory.ServiceCategoryName &&
                s.FacilityId == serviceCategory.FacilityId &&
                s.ServiceCategoryId != serviceCategory.ServiceCategoryId);

            if (duplicate)
                ValidationHelper.AddError(errors, nameof(serviceCategory.ServiceCategoryName), "Service Category Name is already exists in this facility.");

            if (errors.Any())
                throw new ModelValidationException("Validation failed", errors);
        }
    }
}
