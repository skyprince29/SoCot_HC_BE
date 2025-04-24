using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Services
{
    public class ServiceService : Repository<Service, Guid>, IServiceService
    {
        public ServiceService(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Service>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet
                .Include(s => s.Facility)
                .Include(s => s.ServiceClassification)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(s => s.ServiceName.Contains(keyword));
            }

            return await query
                .Skip((pageNo - 1) * limit)
                .Take(limit)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(s => s.ServiceName.Contains(keyword));
            }

            return await query.CountAsync(cancellationToken);
        }

        public async Task SaveServiceAsync(Service service, CancellationToken cancellationToken = default)
        {
            // Determine if new or existing
            bool isNew = service.ServiceId == Guid.Empty;
            ValidateFields(service);

            if (isNew)
            {
                service.ServiceId = Guid.NewGuid();
                service.IsActive = true;
                await AddAsync(service, cancellationToken);
            }
            else
            {
                var existing = await _dbSet.FindAsync(new object[] { service.ServiceId }, cancellationToken);
                if (existing == null)
                    throw new Exception("Service not found.");

                // Replace all fields
                _context.Entry(existing).CurrentValues.SetValues(service);

                await UpdateAsync(existing, cancellationToken);
            }
        }

        private void ValidateFields(Service service)
        {
            var errors = new Dictionary<string, List<string>>();


            int facilityId = service.FacilityId;
            ValidationHelper.IsRequired(errors, nameof(service.FacilityId), facilityId, "Facility");
            // Verify that the FacilityId exists
            var facilityExists = _context.Facility.Any(f => f.FacilityId == facilityId);
            if (!facilityExists && facilityId > 0)
            {
                ValidationHelper.AddError(errors, nameof(service.FacilityId), "Facility is invalid.");
            }

            int serviceClassificationId = service.ServiceClassificationId;
            ValidationHelper.IsRequired(errors, nameof(service.ServiceClassificationId), serviceClassificationId, "Service Classification");
            // Verify that the ServiceClassificationId exists
            var classificationExists = _context.ServiceClassification.Any(sc => sc.ServiceClassificationId == serviceClassificationId);
            if (!classificationExists && serviceClassificationId > 0)
            {
                ValidationHelper.AddError(errors, nameof(service.ServiceClassificationId), "Service Classification is invalid..");
            }

            ValidationHelper.IsRequired(errors, nameof(service.ServiceName), service.ServiceName, "Service Name");
            bool duplicate = _dbSet.Any(s =>
                s.ServiceName == service.ServiceName &&
                s.FacilityId == service.FacilityId &&
                s.ServiceId != service.ServiceId);

            if (duplicate)
                ValidationHelper.AddError(errors, "", "Service is already exists in this facility.");

            if (errors.Any())
                throw new ModelValidationException("Validation failed", errors);
        }
    }
} 