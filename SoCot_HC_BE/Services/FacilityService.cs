using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Model.Enums;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Services
{
    public class FacilityService : Repository<Facility, int>, IFacilityService
    {
        private readonly IAddressService _addressService;

        public FacilityService(AppDbContext context, IAddressService addressService) : base(context)
        {
            _addressService = addressService;
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
            ValidateFields(facility);

            // 🔄 Ensure the Address is unique or retrieved from DB
            if (facility.Address != null)
            {
                facility.Address = await _addressService.GetOrCreateAddressAsync(facility.Address, cancellationToken);
                facility.AddressId = facility.Address.AddressId;
            }

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

        private void ValidateFields(Facility facility)
        {
            var errors = new Dictionary<string, List<string>>();

            ValidationHelper.IsRequired(errors, nameof(facility.Address), facility.Address, "Address");
            ValidationHelper.IsRequired(errors, nameof(facility.FacilityName), facility.FacilityName, "Facility Name");

            bool duplicate = _dbSet.Any(s =>
               s.FacilityName == facility.FacilityName &&
               s.FacilityId != facility.FacilityId);

            if (duplicate)
                ValidationHelper.AddError(errors, nameof(facility.FacilityName), "Facility name already exists.");

            ValidationHelper.IsRequired(errors, nameof(facility.Sector), facility.Sector, "Sector");
            if (!Enum.IsDefined(typeof(Sector), facility.Sector))
            {
                ValidationHelper.AddError(errors, nameof(facility.Sector), "Sector is invalid.");
            }
            ValidationHelper.IsRequired(errors, nameof(facility.FacilityLevel), facility.FacilityLevel, "Facility Level");

            if (!Enum.IsDefined(typeof(FacilityLevel), facility.FacilityLevel))
            {
                ValidationHelper.AddError(errors, nameof(facility.FacilityLevel), "Facility Level is invalid.");
            }

            _addressService.ValidateAddress(facility.Address, errors);

            if (errors.Any())
                throw new ModelValidationException("Validation failed", errors);
        }
    }
}
