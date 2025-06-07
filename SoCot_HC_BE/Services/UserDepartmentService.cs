using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SCHC_API.Handler;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SoCot_HC_BE.Services
{
    public class UserDepartmentService : Repository<UserDepartment, Guid>, IUserDepartmentService
    {
        public UserDepartmentService(AppDbContext context) : base(context)
        {
        }

        public async Task DeactivateUserDepartmentAsync(UserDeptModelDto userDeptModelDto, CancellationToken cancellationToken = default)
        {
            Guid personId = userDeptModelDto.personId;
            List<Guid> departmentIds = userDeptModelDto.departmentIds;

            foreach (var departmentId in departmentIds)
            {
                var data = await _dbSet.FirstOrDefaultAsync(
                    i => (i.PersonId == personId && i.DepartmentId == departmentId), cancellationToken);

                if (data == null)
                {
                    throw new Exception("User department not found.");
                }
                else
                {
                    data.IsActive = false;
                    await UpdateAsync(data, cancellationToken);
                }
            }

        }

        public async Task<PaginationHandler<UserDepartmentDto>> GetAllWithPagingAsync(
            Guid personId, int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet
                           .Include(ud => ud.Person)
                           .Include(ud => ud.Department)
                           .Where(ud => ud.IsActive)
                           .AsNoTracking() 
                           .AsQueryable();

            if (personId != Guid.Empty)
            {
                query = query.Where(ud => ud.PersonId == personId);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string lowerKeyword = keyword.ToLower().Trim(); // Convert keyword to lowercase and trim once

                query = query.Where(ud =>
                    // Check Person's name fields (null-safe and case-insensitive)
                    (ud.Person != null && ud.Person.Firstname != null && ud.Person.Firstname.ToLower().Contains(lowerKeyword)) ||
                    (ud.Person != null && ud.Person.Middlename != null && ud.Person.Middlename.ToLower().Contains(lowerKeyword)) ||
                    (ud.Person != null && ud.Person.Lastname != null && ud.Person.Lastname.ToLower().Contains(lowerKeyword)) 
                );
            }

            // Get total records *before* applying Skip/Take
            int totalRecords = await query.CountAsync(cancellationToken);

            var userDepartmentDtos = await query
               .OrderBy(ud => ud.Person.Lastname) // Example ordering for consistent pagination
               .Skip((pageNo - 1) * limit)
               .Take(limit)
               .Select(ud => new UserDepartmentDto
               {
                   UserDepartmentId = ud.UserDepartmentId,
                   PersonId = ud.PersonId,
                   Firstname = ud.Person != null ? ud.Person.Firstname : string.Empty,
                   Middlename = ud.Person != null ? ud.Person.Middlename : string.Empty,
                   Lastname = ud.Person != null ? ud.Person.Lastname : string.Empty,
                   DepartmentId = ud.DepartmentId,
                   DepartmentCode = ud.Department != null ? ud.Department.DepartmentCode : string.Empty, 
                   DepartmentName = ud.Department != null ? ud.Department.DepartmentName : string.Empty,
                   IsActive = ud.IsActive,
               })
               .ToListAsync(cancellationToken);


            var paginatedResult = new PaginationHandler<UserDepartmentDto>(userDepartmentDtos, totalRecords, pageNo, limit);
            return paginatedResult!;
        }

        public async Task SaveUserDepartmentAsync(UserDeptModelDto userDeptModelDto, CancellationToken cancellationToken = default)
        {

            Guid personId = userDeptModelDto.personId;
            List<Guid> departmentIds = userDeptModelDto.departmentIds;

            foreach (var departmentId in departmentIds) {
                var data = await _dbSet.FirstOrDefaultAsync(
                    i => (i.PersonId == personId && i.DepartmentId == departmentId), cancellationToken);

                if (data == null)
                {
                    UserDepartment userDepartment = new UserDepartment();
                    userDepartment.PersonId = personId;
                    userDepartment.DepartmentId = departmentId;
                    userDepartment.IsActive = true;
                    _dbSet.Add(userDepartment);
                    await AddAsync(userDepartment, cancellationToken);
                }
                else {
                    data.IsActive = true;
                    await UpdateAsync(data, cancellationToken);
                }
            }
        }



        // Get a list of VitalSigns with paging and cancellation support.
        //public async Task<List<VitalSign>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default)
        //{
        //    var query = _dbSet.AsQueryable();

        //    return await query
        //        .Skip((pageNo - 1) * limit)
        //        .Take(limit)
        //        .ToListAsync(cancellationToken); // Pass the CancellationToken here
        //}

        //// Count the number of VitalSigns, supporting cancellation.
        //public async Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default)
        //{
        //    var query = _dbSet.AsQueryable();
        //    return await query.CountAsync(cancellationToken); // Pass the CancellationToken here
        //}

        //// Optional: Get all VitalSigns without cancellation support (not recommended for production)
        //public async Task<List<VitalSign>> GetAllWithoutTokenAsync()
        //{
        //    return await _dbSet.ToListAsync();
        //}

        //private VitalSign DTOToModel(VitalSignDto dto)
        //{
        //    if (dto == null)
        //        throw new ArgumentNullException(nameof(dto));

        //    return new VitalSign
        //    {
        //        VitalSignId = dto.VitalSignId == Guid.Empty ? Guid.Empty : dto.VitalSignId,
        //        PatientRegistryId = (dto.PatientRegistryId.HasValue ?
        //            (dto.PatientRegistryId == Guid.Empty ? dto.PatientRegistryId : null) : null),
        //        Temperature = dto.Temperature,
        //        Height = dto.Height,
        //        Weight = dto.Weight,
        //        RespiratoryRate = dto.RespiratoryRate,
        //        CardiacRate = dto.CardiacRate,
        //        Systolic = dto.Systolic,
        //        Diastolic = dto.Diastolic,
        //        BloodPressure = dto.Systolic + "/" + dto.Diastolic,

        //        // Copying base audit properties
        //        CreatedBy = dto.CreatedBy,
        //        CreatedDate = dto.CreatedDate,
        //        UpdatedBy = dto.UpdatedBy,
        //        UpdatedDate = dto.UpdatedDate
        //    };
        //}

        //public async Task SaveVitalSignAsync(VitalSignDto vitalSignDto, CancellationToken cancellationToken = default)
        //{
        //    ValidateFields(vitalSignDto);
        //    VitalSign vitalSign = DTOToModel(vitalSignDto);
        //    // Determine if new or existing
        //    bool isNew = vitalSign.VitalSignId == Guid.Empty;
        //    if (isNew)
        //    {
        //        vitalSign.VitalSignId = Guid.NewGuid();
        //        await AddAsync(vitalSign, cancellationToken);
        //    }
        //    else
        //    {
        //        var existing = await _dbSet.FindAsync(new object[] { vitalSign.VitalSignId }, cancellationToken);
        //        if (existing == null)
        //            throw new Exception("Vital Sign not found.");

        //        // Replace all fields
        //        _context.Entry(existing).CurrentValues.SetValues(vitalSign);

        //        await UpdateAsync(existing, cancellationToken);
        //    }
        //}

        //private void ValidateFields(VitalSignDto vitalSignDto)
        //{
        //    var errors = new Dictionary<string, List<string>>();
        //    Guid? patientRegistryId = vitalSignDto.PatientRegistryId;
        //    if (patientRegistryId.HasValue && patientRegistryId.Value != Guid.Empty)
        //    {
        //        var ptExists = _context.PatientRegistry.Any(f => f.PatientRegistryId == patientRegistryId);
        //        if (ptExists)
        //        {
        //            ValidationHelper.AddError(errors, nameof(vitalSignDto.PatientRegistryId), "Patient registry is invalid.");
        //        }
        //    }

        //    ValidationHelper.IsRequired(errors, nameof(vitalSignDto.Systolic), vitalSignDto.Systolic, "Systolic");
        //    ValidationHelper.IsRequired(errors, nameof(vitalSignDto.Diastolic), vitalSignDto.Diastolic, "Diastolic");
        //    Decimal? temperature = vitalSignDto.Temperature;
        //    if (temperature!= null && temperature.Value <= 0)
        //    {
        //        ValidationHelper.AddError(errors, nameof(vitalSignDto.Temperature), "Temperature is invalid value.");
        //    }
        //    Decimal? height = vitalSignDto.Height;
        //    if (height != null && height.Value <= 0)
        //    {
        //        ValidationHelper.AddError(errors, nameof(vitalSignDto.Height), "Height is required.");
        //    }
        //    Decimal? weight = vitalSignDto.Weight;
        //    if (height != null && height <= 0)
        //    {
        //        ValidationHelper.AddError(errors, nameof(vitalSignDto.Weight), "Weight is required.");
        //    }
        //    int? cardiacRate = vitalSignDto.CardiacRate;
        //    if (cardiacRate != null && cardiacRate.Value <= 0)
        //    {
        //        ValidationHelper.AddError(errors, nameof(vitalSignDto.CardiacRate), "Cardiac Rate is invalid value.");
        //    }
        //    int? respiratoryRate = vitalSignDto.RespiratoryRate;
        //    if (respiratoryRate != null && respiratoryRate.Value <= 0)
        //    {
        //        ValidationHelper.AddError(errors, nameof(vitalSignDto.RespiratoryRate), "Respiratory Rate is invalid value.");
        //    }
        //    if (errors.Any())
        //        throw new ModelValidationException("Validation failed", errors);
        //}
    }
}
