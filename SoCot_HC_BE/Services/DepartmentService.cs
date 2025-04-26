using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Services
{
    public class DepartmentService : Repository<Department, Guid>, IDepartmentService
    {
        public DepartmentService(AppDbContext context) : base(context)
        {
        }

        public override async Task<Department?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {

            // You can include related data here if needed, like navigation properties
            var department = await _dbSet
                .Include(f => f.DepartmentTypes)
                    .ThenInclude(dt => dt.DepartmentType) // ✅ include the inner object
                .FirstOrDefaultAsync(f => f.DepartmentId == id, cancellationToken);

            return department;
        }

        public async Task<List<Department>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet
                 .Include(f => f.DepartmentTypes)
                    .ThenInclude(dt => dt.DepartmentType) // ✅ include the inner object
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(v => v.DepartmentName.Contains(keyword)); // You can adjust this to your need
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
                        .Where(v => v.DepartmentName.Contains(keyword));
            }
            return await query.CountAsync(cancellationToken); // Pass the CancellationToken here
        }

        public async Task<List<Department>> GetAllActiveOnlyAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(s => s.IsActive)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Department>> GetAllActiveWithCurrentAsync(Guid currentId, CancellationToken cancellationToken = default)
        {
            var activeItems = await _dbSet
                     .Where(s => s.IsActive)
                     .ToListAsync(cancellationToken);

            // Check if the currentId is not among the active items
            bool currentExists = activeItems.Any(s => s.DepartmentId == currentId);

            if (!currentExists)
            {
                var currentItem = await _dbSet
                    .FirstOrDefaultAsync(s => s.DepartmentId == currentId, cancellationToken);

                if (currentItem != null)
                    activeItems.Add(currentItem);
            }

            return activeItems;
        }

        public async Task SaveDepartmentAsync(Department department, CancellationToken cancellationToken = default)
        {
            bool isNew = department.DepartmentId == Guid.Empty;

            if (isNew)
            {
                department.DepartmentId = Guid.NewGuid();
                department.CreatedDate = DateTime.Now;
                department.DepartmentCode = await GenerateDepartmentCodeAsync(department.FacilityId, cancellationToken);
            }

            // 🔍 Validate fields after DepartmentCode is generated
            ValidateFields(department);

            if (isNew)
            {
                await AddAsync(department, cancellationToken);
            }
            else
            {
                var existingDepartment = await _dbSet.FindAsync(new object[] { department.DepartmentId }, cancellationToken);

                if (existingDepartment == null)
                    throw new Exception("Department not found.");

                _context.Entry(existingDepartment).CurrentValues.SetValues(department);

                SaveUpdateDepartmentDepartmentTypes(existingDepartment, department);

                await UpdateAsync(existingDepartment, cancellationToken);
            }
        }

        private void SaveUpdateDepartmentDepartmentTypes(Department existingDepartment, Department updatedDepartment)
        {
            // Directly access the join table from context
            var departmentTypeSet = _context.Set<DepartmentDepartmentType>();

            // Get existing and updated type IDs
            var existingTypeIds = existingDepartment.DepartmentTypes.Select(s => s.DepartmentTypeId).ToList();
            var updatedTypeIds = updatedDepartment.DepartmentTypes.Select(s => s.DepartmentTypeId).ToList();

            // Remove ones not in the updated list
            foreach (var existing in existingDepartment.DepartmentTypes.ToList())
            {
                if (!updatedTypeIds.Contains(existing.DepartmentTypeId))
                {
                    departmentTypeSet.Remove(existing);
                }
            }

            // Add new ones from updated list
            foreach (var updated in updatedDepartment.DepartmentTypes)
            {
                if (!existingTypeIds.Contains(updated.DepartmentTypeId))
                {
                    updated.DepartmentId = existingDepartment.DepartmentId;
                    departmentTypeSet.Add(updated);
                }
            }
        }

        private async Task<string> GenerateDepartmentCodeAsync(int facilityId, CancellationToken cancellationToken)
        {
            var lastCode = await _dbSet
                .Where(d => d.FacilityId == facilityId)
                .OrderByDescending(d => d.DepartmentCode)
                .Select(d => d.DepartmentCode)
                .FirstOrDefaultAsync(cancellationToken);

            int nextNumber = 1;
            if (!string.IsNullOrEmpty(lastCode) && int.TryParse(lastCode, out int parsed))
            {
                nextNumber = parsed + 1;
            }

            return nextNumber.ToString("D3"); // Pads with leading zeroes (e.g., 001, 002, ...)
        }


        private void ValidateFields(Department department)
        {
            var errors = new Dictionary<string, List<string>>();

            // Required: Department Name
            ValidationHelper.IsRequired(errors, nameof(department.DepartmentName), department.DepartmentName, "Department Name");

            // Duplicate Check: Department Name (within same facility)
            bool duplicate = _dbSet.Any(s =>
                s.DepartmentName == department.DepartmentName &&
                s.FacilityId == department.FacilityId &&
                s.DepartmentId != department.DepartmentId);

            if (duplicate)
                ValidationHelper.AddError(errors, nameof(department.DepartmentName), "Department name already exists in this facility.");

            // Required: Department Code
            ValidationHelper.IsRequired(errors, nameof(department.DepartmentCode), department.DepartmentCode, "Department Code");
            bool duplicateCode = _dbSet.Any(d =>
                d.FacilityId == department.FacilityId &&
                d.DepartmentCode == department.DepartmentCode &&
                d.DepartmentId != department.DepartmentId);

            if (duplicateCode)
            {
                ValidationHelper.AddError(errors, nameof(department.DepartmentCode), "Department code already exists in this facility.");
            }

            ValidationHelper.IsRequired(errors, nameof(department.FacilityId), department.FacilityId, "Facility");

            // Parent Department validation
            if (department.ParentDepartmentId.HasValue)
            {
                if (department.ParentDepartmentId == department.DepartmentId)
                {
                    ValidationHelper.AddError(errors, nameof(department.ParentDepartmentId), "Department cannot be its own parent.");
                }

                // Check if parent department exists
                bool parentExists = _dbSet.Any(d => d.DepartmentId == department.ParentDepartmentId);
                if (!parentExists)
                {
                    ValidationHelper.AddError(errors, nameof(department.ParentDepartmentId), "Parent department is invalid.");
                }
            }

            // Department Types validation (required list)
            if (department.DepartmentTypes == null || !department.DepartmentTypes.Any())
            {
                ValidationHelper.AddError(errors, nameof(department.DepartmentTypes), "At least one department type is required.");
            }
            else
            {
                var departmentTypeIds = new HashSet<Guid>();
                // Optionally, validate the individual department types (if necessary)
                foreach (var departmentType in department.DepartmentTypes)
                {
                    if (departmentType.DepartmentTypeId == Guid.Empty)
                    {
                        ValidationHelper.AddError(errors, nameof(department.DepartmentTypes), "Department type is invalid.");
                    }
                    else if (!departmentTypeIds.Add(departmentType.DepartmentTypeId))
                    {
                        ValidationHelper.AddError(errors, nameof(department.DepartmentTypes), "Duplicate department type found.");
                    }
                }
            }


            // Throw if there are any errors
            if (errors.Any())
                throw new ModelValidationException("Validation failed", errors);
        }


    }
}
