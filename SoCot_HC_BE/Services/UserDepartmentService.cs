using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SCHC_API.Handler;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SoCot_HC_BE.Services
{
    public class UserDepartmentService : Repository<UserDepartment, Guid>, IUserDepartmentService
    {
        public UserDepartmentService(AppDbContext context) : base(context)
        {
        }

        public async Task DeactivateOrActivateUserDepartmentAsync(UserDeptModelDto userDeptModelDto, CancellationToken cancellationToken = default)
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
                    data.IsActive = !data.IsActive;
                    await UpdateAsync(data, cancellationToken);
                }
            }

        }

        public async Task<PaginationHandler<UserDepartmentDto>> GetAllWithPagingAsync(
            Guid personId, int pageNo, int limit, string? keyword = null, bool? isActive = true, CancellationToken cancellationToken = default)
        {
            var query = _dbSet
                           .Include(ud => ud.Person)
                           .Include(ud => ud.Department)
                           .AsNoTracking() 
                           .AsQueryable();

            if (isActive.HasValue)
            {
                query = query.Where(ud => ud.IsActive == isActive);
            }
            else
            {
                query = query.Where(ud => ud.IsActive);
            }

            if (personId != Guid.Empty)
            {
                query = query.Where(ud => ud.PersonId == personId);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string lowerKeyword = keyword.ToLower().Trim(); 
                query = query.Where(ud => (ud.Department != null && ud.Department.DepartmentName.ToLower().Contains(lowerKeyword)));
            }

            int totalRecords = await query.CountAsync(cancellationToken);

            var userDepartmentDtos = await query
               .OrderBy(ud => ud.Person.Lastname) 
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

        public async Task<List<Department>> GetDepartmentsByUser(
            Guid personId, CancellationToken cancellationToken = default)
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

            var userDepartmentDtos = await query
               .OrderBy(ud => ud.Department.DepartmentName) 
               .Select(ud => new Department
               {
                   DepartmentId = (Guid)(ud.DepartmentId != null ? ud.DepartmentId : Guid.Empty),
                   DepartmentCode = ud.Department != null ? ud.Department.DepartmentCode : string.Empty,
                   DepartmentName = ud.Department != null ? ud.Department.DepartmentName : string.Empty,
                   IsActive = ud.IsActive,
               })
               .ToListAsync(cancellationToken);

          return userDepartmentDtos!;
        }


        public async Task<PaginationHandler<UserDepartmentAssignedDto>> GetAllWithPagingUserOnUserDepartmentAsync(
         int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default)
        {
          
            var baseQuery = _context.UserDepartment
             .AsNoTracking()
             .Include(ud => ud.Person)
             .Include(ud => ud.Department) 
             .Where(ud => ud.Person != null);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string lowerKeyword = keyword.ToLower().Trim();

                baseQuery = baseQuery.Where(ud =>
                    (ud.Department != null && ud.Department.DepartmentName.ToLower().Contains(lowerKeyword)) ||
                    (ud.Person.Firstname != null && ud.Person.Firstname.ToLower().Contains(lowerKeyword)) ||
                    (ud.Person.Middlename != null && ud.Person.Middlename.ToLower().Contains(lowerKeyword)) ||
                    (ud.Person.Lastname != null && ud.Person.Lastname.ToLower().Contains(lowerKeyword))
                );
            }

            var groupedQuery = baseQuery.GroupBy(ud => ud.PersonId);
            int totalRecords = await groupedQuery.CountAsync(cancellationToken); 

            var userDepartmentAssignedDtos = groupedQuery
                .Select(group => new UserDepartmentAssignedDto 
                {
                    UserDepartmentId = group.FirstOrDefault().UserDepartmentId,
                    PersonId = group.Key,
                    Firstname = group.FirstOrDefault().Person.Firstname ?? string.Empty,
                    Middlename = group.FirstOrDefault().Person.Middlename ?? string.Empty,
                    Lastname = group.FirstOrDefault().Person.Lastname ?? string.Empty,
                    TotalAssignedDepartments = group.Count(ud => ud.IsActive)
                })
                .AsEnumerable() 
                .OrderBy(dto => dto.Lastname) 
                .ThenBy(dto => dto.Firstname)
                .Skip((pageNo - 1) * limit)
                .Take(limit)
                .ToList(); 

            var paginatedResult = new PaginationHandler<UserDepartmentAssignedDto>(userDepartmentAssignedDtos, totalRecords, pageNo, limit);
            return paginatedResult;
        }
    }
}
