using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.Designations.Interfaces;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Model.Enums;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Services
{
    public class DesignationService : Repository<Designation, Guid>, IDesignationService
    {
        private readonly IAddressService _addressService;

        public DesignationService(AppDbContext context, IAddressService addressService) : base(context)
        {
            _addressService = addressService;
        }
        public override async Task<Designation?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // You can include related data here if needed, like navigation properties
            var designation = await _dbSet
                .FirstOrDefaultAsync(f => f.DesignationId == id, cancellationToken);

            return designation;
        }

        // Get a list of Designation with paging and cancellation support.
        public async Task<List<Designation>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(v => v.DesignationName.Contains(keyword)); // You can adjust this to your need
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
                        .Where(v => v.DesignationName.Contains(keyword));
            }
            return await query.CountAsync(cancellationToken); // Pass the CancellationToken here
        }

        public async Task<List<Designation>> GetAllActiveOnlyAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(s => s.IsActive)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Designation>> GetAllActiveWithCurrentAsync(Guid currentId, CancellationToken cancellationToken = default)
        {
            var activeItems = await _dbSet
                     .Where(s => s.IsActive)
                     .ToListAsync(cancellationToken);

            // Check if the currentId is not among the active items
            bool currentExists = activeItems.Any(s => s.DesignationId == currentId);

            if (!currentExists)
            {
                var currentItem = await _dbSet
                    .FirstOrDefaultAsync(s => s.DesignationId == currentId, cancellationToken);

                if (currentItem != null)
                    activeItems.Add(currentItem);
            }

            return activeItems;
        }

        public async Task SaveDesignationAsync(Designation designation, CancellationToken cancellationToken = default)
        {
            // Determine if new or existing
            bool isNew = designation.DesignationId == Guid.Empty;
            ValidateFields(designation);


            if (isNew)
            {
                string timestamp = DateTime.Now.ToString("yyMMdd-HHmmss");
                //TODO: Update code
                designation.DesignationCode = $"001-{timestamp}";
                await AddAsync(designation, cancellationToken);
            }
            else
            {
                var existing = await _dbSet.FindAsync(new object[] { designation.DesignationId }, cancellationToken);
                if (existing == null)
                    throw new Exception("Designation not found.");

                // Replace all fields
                _context.Entry(existing).CurrentValues.SetValues(designation);

                await UpdateAsync(existing, cancellationToken);
            }
        }

        private void ValidateFields(Designation designation)
        {
            var errors = new Dictionary<string, List<string>>();

            string designationName = designation.DesignationName;
            ValidationHelper.IsRequired(errors, nameof(designation.DesignationName), designationName, "Designation Name");

            bool duplicate = _dbSet.Any(s =>
               s.DesignationName == designationName &&
               s.DesignationId != designation.DesignationId);

            if (duplicate)
                ValidationHelper.AddError(errors, nameof(designation.DesignationName), "Designation name already exists.");
            if (errors.Any())
                throw new ModelValidationException("Validation failed", errors);
        }
    }
}
