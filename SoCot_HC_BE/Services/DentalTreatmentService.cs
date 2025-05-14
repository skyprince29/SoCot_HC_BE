using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SCHC_API.Handler;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SoCot_HC_BE.Services
{
    public class DentalTreatmentService : Repository<DentalTreatment, Guid>, IDentalTreatmentService
    {
        public DentalTreatmentService(AppDbContext context) : base(context)
        {
      
        }

        //public override async Task<Department?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        //{

        //    // You can include related data here if needed, like navigation properties
        //    var department = await _dbSet
        //        .Include(f => f.DepartmentTypes)
        //            .ThenInclude(dt => dt.DepartmentType) // ✅ include the inner object
        //        .FirstOrDefaultAsync(f => f.DepartmentId == id, cancellationToken);

        //    return department;
        //}

        public async Task<PaginationHandler<DentalTreatment>> GetAllWithPagingAsync(int pageNo, int limit, string keyword = "", CancellationToken cancellationToken = default)
        {
            var dentalTreatment = await _dbSet
                .Include(f => f.Facility)
                .Include(p => p.Patient)
                .Include(pr => pr.PatientRegistry)
                .Where(
                dt =>
                dt.Patient.Firstname.Contains(keyword) ||
                dt.Patient.Lastname.Contains(keyword) ||
                (dt.Patient.Middlename != null && dt.Patient.Middlename.Contains(keyword))
                )
               .AsNoTracking()
               .ToListAsync();

            int totalRecords = dentalTreatment.Count;


            var paginatedResult = new PaginationHandler<DentalTreatment>(dentalTreatment, totalRecords, pageNo, limit);
            return paginatedResult;
        }

        //public async Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default)
        //{
        //    var query = _dbSet.AsQueryable();

        //    if (!string.IsNullOrEmpty(keyword))
        //    {
        //        query = query
        //                .Where(v => v.DepartmentName.Contains(keyword));
        //    }
        //    return await query.CountAsync(cancellationToken); // Pass the CancellationToken here
        //}

        //public async Task<List<Department>> GetAllActiveOnlyAsync(CancellationToken cancellationToken = default)
        //{
        //    return await _dbSet
        //        .Where(s => s.IsActive)
        //        .ToListAsync(cancellationToken);
        //}

        //public async Task<List<Department>> GetAllActiveWithCurrentAsync(Guid currentId, CancellationToken cancellationToken = default)
        //{
        //    var activeItems = await _dbSet
        //             .Where(s => s.IsActive)
        //             .ToListAsync(cancellationToken);

        //    // Check if the currentId is not among the active items
        //    bool currentExists = activeItems.Any(s => s.DepartmentId == currentId);

        //    if (!currentExists)
        //    {
        //        var currentItem = await _dbSet
        //            .FirstOrDefaultAsync(s => s.DepartmentId == currentId, cancellationToken);

        //        if (currentItem != null)
        //            activeItems.Add(currentItem);
        //    }

        //    return activeItems;
        //}

        //public async Task SaveDepartmentAsync(DepartmentDTO departmentDTO, CancellationToken cancellationToken = default)
        //{

        //    bool isNew = departmentDTO.DepartmentId == Guid.Empty;

        //    // 🔍 Validate fields after DepartmentCode is generated
        //    ValidateFields(departmentDTO);

        //    var department = new Department
        //    {
        //        DepartmentCode = await GenerateDepartmentCodeAsync(departmentDTO.FacilityId, cancellationToken),
        //        DepartmentId = departmentDTO.DepartmentId,
        //        FacilityId = departmentDTO.FacilityId,
        //        DepartmentName = departmentDTO.DepartmentName,
        //        Description = departmentDTO.Description,
        //        ParentDepartmentId = departmentDTO.ParentDepartmentId,
        //        IsReferable = departmentDTO.IsReferable,
        //        IsActive = departmentDTO.IsActive,
        //        DepartmentTypes = new List<DepartmentDepartmentType>()
        //    };

        //    if (isNew)
        //    {
        //        // Set DepartmentTypes directly to the new entity
        //        foreach (var typeId in departmentDTO.DepartmentTypeIds)
        //        {
        //            department.DepartmentTypes.Add(new DepartmentDepartmentType
        //            {
        //                DepartmentId = department.DepartmentId,
        //                DepartmentTypeId = typeId
        //            });
        //        }

        //        await AddAsync(department, cancellationToken);
        //    }
        //    else
        //    {
        //        var existingDepartment = await _dbSet
        //            .Include(d => d.DepartmentTypes)
        //            .FirstOrDefaultAsync(d => d.DepartmentId == department.DepartmentId, cancellationToken);

        //        if (existingDepartment == null)
        //            throw new Exception("Department not found.");

        //        _context.Entry(existingDepartment).CurrentValues.SetValues(department);

        //        SaveUpdateDepartmentDepartmentTypes(existingDepartment, departmentDTO.DepartmentTypeIds);
        //        await UpdateAsync(existingDepartment, cancellationToken);
        //    }
        //}

        //private void SaveUpdateDepartmentDepartmentTypes(Department existingDepartment, List<Guid> departmentTypeIds)
        //{
        //    var departmentTypeSet = _context.Set<DepartmentDepartmentType>();

        //    // Remove all existing department-type relationships
        //    foreach (var existing in existingDepartment.DepartmentTypes.ToList())
        //    {
        //        departmentTypeSet.Remove(existing);
        //    }

        //    // Add new department-type relationships
        //    foreach (var typeId in departmentTypeIds)
        //    {
        //        departmentTypeSet.Add(new DepartmentDepartmentType
        //        {
        //            DepartmentId = existingDepartment.DepartmentId,
        //            DepartmentTypeId = typeId
        //        });
        //    }
        //}

        //private async Task<string> GenerateDepartmentCodeAsync(int facilityId, CancellationToken cancellationToken)
        //{
        //    var lastCode = await _dbSet
        //        .Where(d => d.FacilityId == facilityId)
        //        .OrderByDescending(d => d.DepartmentCode)
        //        .Select(d => d.DepartmentCode)
        //        .FirstOrDefaultAsync(cancellationToken);

        //    int nextNumber = 1;
        //    if (!string.IsNullOrEmpty(lastCode) && int.TryParse(lastCode, out int parsed))
        //    {
        //        nextNumber = parsed + 1;
        //    }

        //    return nextNumber.ToString("D3"); // Pads with leading zeroes (e.g., 001, 002, ...)
        //}


        //private void ValidateFields(DepartmentDTO department)
        //{
        //    var errors = new Dictionary<string, List<string>>();

        //    // Required: Department Name
        //    ValidationHelper.IsRequired(errors, nameof(department.DepartmentName), department.DepartmentName, "Department Name");

        //    // Duplicate Check: Department Name (within same facility)
        //    bool duplicate = _dbSet.Any(s =>
        //        s.DepartmentName == department.DepartmentName &&
        //        s.FacilityId == department.FacilityId &&
        //        s.DepartmentId != department.DepartmentId);

        //    if (duplicate)
        //        ValidationHelper.AddError(errors, nameof(department.DepartmentName), "Department name already exists in this facility.");

        //    // Required: Department Code
        //    //ValidationHelper.IsRequired(errors, nameof(department.DepartmentCode), department.DepartmentCode, "Department Code");
        //    //bool duplicateCode = _dbSet.Any(d =>
        //    //    d.FacilityId == department.FacilityId &&
        //    //    d.DepartmentCode == department.DepartmentCode &&
        //    //    d.DepartmentId != department.DepartmentId);

        //    //if (duplicateCode)
        //    //{
        //    //    ValidationHelper.AddError(errors, nameof(department.DepartmentCode), "Department code already exists in this facility.");
        //    //}

        //    ValidationHelper.IsRequired(errors, nameof(department.FacilityId), department.FacilityId, "Facility");

        //    // Parent Department validation
        //    if (department.ParentDepartmentId.HasValue)
        //    {
        //        if (department.ParentDepartmentId == department.DepartmentId)
        //        {
        //            ValidationHelper.AddError(errors, nameof(department.ParentDepartmentId), "Department cannot be its own parent.");
        //        }

        //        // Check if parent department exists
        //        bool parentExists = _dbSet.Any(d => d.DepartmentId == department.ParentDepartmentId);
        //        if (!parentExists)
        //        {
        //            ValidationHelper.AddError(errors, nameof(department.ParentDepartmentId), "Parent department is invalid.");
        //        }
        //    }

        //    // Department Types validation (required list)
        //    if (department.DepartmentTypeIds == null || !department.DepartmentTypeIds.Any())
        //    {
        //        ValidationHelper.AddError(errors, nameof(department.DepartmentTypeIds), "At least one department type is required.");
        //    }

        //    // Throw if there are any errors
        //    if (errors.Any())
        //        throw new ModelValidationException("Validation failed", errors);
        //}


    }
}
