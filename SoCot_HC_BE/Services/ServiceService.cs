using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Dtos;
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

        public override async Task<Service?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {

            // You can include related data here if needed, like navigation properties
            var department = await _dbSet
                .Include(f => f.ServiceDepartments)
                    .ThenInclude(dt => dt.Department) // ✅ include the inner object
                .FirstOrDefaultAsync(f => f.ServiceId == id, cancellationToken);

            return department;
        }

        public async Task<List<Service>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet
                .Include(s => s.Facility)
                .Include(s => s.ServiceClassification)
                .Include(f => f.ServiceDepartments)
                                .ThenInclude(dt => dt.Department)
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

        private Service DTOToModel(ServiceDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            var service = new Service
            {
                ServiceId = dto.ServiceId == Guid.Empty ? Guid.Empty : dto.ServiceId,
                ServiceName = dto.ServiceName ?? string.Empty,
                Description = dto.Description,
                FacilityId = dto.FacilityId,
                DepartmentId = dto.DepartmentId,
                ServiceClassificationId = dto.ServiceClassificationId,
                ServiceCategoryId = dto.ServiceCategoryId,
                IsActive = dto.IsActive,
                ServiceDepartments = dto.DepartmentIds?
                    .Select(deptId => new ServiceDepartment
                    {
                        ServiceDepartmentId = Guid.NewGuid(),
                        DepartmentId = deptId,
                        IsActive = true // assuming newly added departments are active
                    }).ToList() ?? new List<ServiceDepartment>(),
            };
            return service;
        }

        public async Task SaveServiceAsync(ServiceDto serviceDto, CancellationToken cancellationToken = default)
        {
            // Determine if new or existing
            bool isNew = serviceDto.ServiceId == Guid.Empty;
            ValidateFields(serviceDto);
            Service service =  DTOToModel(serviceDto);

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
                SaveOrUpdateServiceDepartments(existing, serviceDto.DepartmentIds);

                await UpdateAsync(existing, cancellationToken);
            }
        }

        private void SaveOrUpdateServiceDepartments(Service existingService, List<Guid> newDepartmentIds)
        {
            var serviceDepartmentSet = _context.Set<ServiceDepartment>();

            foreach (var departmentId in newDepartmentIds)
            {
                if (existingService.ServiceDepartments != null)
                {
                    var existingRelation = existingService.ServiceDepartments
                    .FirstOrDefault(sd => sd.DepartmentId == departmentId);

                    if (existingRelation != null)
                    {
                        // Reactivate if currently inactive
                        if (!existingRelation.IsActive)
                        {
                            existingRelation.IsActive = true;
                        }
                    }
                    else
                    {
                        // Add new relationship
                        serviceDepartmentSet.Add(new ServiceDepartment
                        {
                            ServiceDepartmentId = Guid.NewGuid(),
                            ServiceId = existingService.ServiceId,
                            DepartmentId = departmentId,
                            IsActive = true
                        });
                    }
                } else
                {
                    // Add new relationship
                    serviceDepartmentSet.Add(new ServiceDepartment
                    {
                        ServiceDepartmentId = Guid.NewGuid(),
                        ServiceId = existingService.ServiceId,
                        DepartmentId = departmentId,
                        IsActive = true
                    });
                }
            }

            if(existingService.ServiceDepartments != null)
            {
                // Deactivate those that are not in the new list
                foreach (var existing in existingService.ServiceDepartments)
                {
                    if (!newDepartmentIds.Contains(existing.DepartmentId))
                    {
                        existing.IsActive = false;
                    }
                }
            }

        }

        private void ValidateFields(ServiceDto service)
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

        public async Task<Department?> GetDepartmentByServiceIdAsync(Guid serviceId, CancellationToken cancellationToken = default)
        {
            var service = await _dbSet
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => s.ServiceId == serviceId, cancellationToken);

            return service?.Department;
        }

        public async Task<List<Department>> GetDepartmentFlowsByServiceIdAsync(
            Guid serviceId,
            Guid? excludeDepartmentId,
            CancellationToken cancellationToken = default)
        {
            var service = await _dbSet
                .AsNoTracking()
                .Include(s => s.ServiceDepartments)
                    .ThenInclude(sd => sd.Department)
                .FirstOrDefaultAsync(s => s.ServiceId == serviceId, cancellationToken);

            if (service == null)
                return new List<Department>();

            return service.ServiceDepartments
                .Where(sd => sd.Department != null &&
                            (!excludeDepartmentId.HasValue || sd.Department.DepartmentId != excludeDepartmentId.Value))
                .Select(sd => sd.Department!)
                .ToList();
        }


        public async Task<List<Service>> GetServicesByDepartment(Guid departmentId, CancellationToken cancellationToken = default)
        {
            var services = await _dbSet
                        .Where(s => s.DepartmentId == departmentId
                        && s.IsActive)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

            return services;
        }

        public async Task<List<Service>> GetServicesByFacility(int facilityId, CancellationToken cancellationToken = default)
        {
            var services = await _dbSet
                       .Where(s => s.FacilityId == facilityId
                       && s.IsActive)
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);
            return services;
        }
    }
} 