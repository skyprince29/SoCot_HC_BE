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
            Guid userAccountId = userDeptModelDto.userAccountId;
            List<Guid> departmentIds = userDeptModelDto.departmentIds;

            foreach (var departmentId in departmentIds)
            {
                var data = await _dbSet.FirstOrDefaultAsync(
                    i => (i.UserAccountId == userAccountId && i.DepartmentId == departmentId), cancellationToken);

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
                           .Include(ud => ud.UserAccount)
                           .ThenInclude(ud => ud.PersonAsUserAccount)
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
                query = query.Where(ud => ud.UserAccount.PersonId == personId);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string lowerKeyword = keyword.ToLower().Trim();
                query = query.Where(ud => (ud.Department != null && ud.Department.DepartmentName.ToLower().Contains(lowerKeyword)));
            }

            int totalRecords = await query.CountAsync(cancellationToken);

            var userDepartmentDtos = await query
               .OrderBy(ud => ud.UserAccount.PersonAsUserAccount.Lastname)
               .Skip((pageNo - 1) * limit)
               .Take(limit)
               .Select(ud => new UserDepartmentDto
               {
                   UserDepartmentId = ud.UserDepartmentId,
                   PersonId = ud.UserAccount.PersonAsUserAccount.PersonId,
                   Firstname = ud.UserAccount.PersonAsUserAccount != null ? ud.UserAccount.PersonAsUserAccount.Firstname : string.Empty,
                   Middlename = ud.UserAccount.PersonAsUserAccount != null ? ud.UserAccount.PersonAsUserAccount.Middlename : string.Empty,
                   Lastname = ud.UserAccount.PersonAsUserAccount != null ? ud.UserAccount.PersonAsUserAccount.Lastname : string.Empty,
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
            Guid userAccountId = userDeptModelDto.userAccountId;
            List<Guid> departmentIds = userDeptModelDto.departmentIds;

            foreach (var departmentId in departmentIds)
            {
                var data = await _dbSet.FirstOrDefaultAsync(
                    i => (i.UserAccountId == userAccountId && i.DepartmentId == departmentId), cancellationToken);

                if (data == null)
                {
                    UserDepartment userDepartment = new UserDepartment();
                    userDepartment.UserAccountId = userAccountId;
                    userDepartment.DepartmentId = departmentId;
                    userDepartment.IsActive = true;
                    _dbSet.Add(userDepartment);
                    await AddAsync(userDepartment, cancellationToken);
                }
                else
                {
                    data.IsActive = true;
                    await UpdateAsync(data, cancellationToken);
                }
            }
        }

        //public async Task<List<Department>> GetDepartmentsByUser(
        //    Guid personId, CancellationToken cancellationToken = default)
        //{
        //    var query = _dbSet
        //                   .Include(ud => ud.Person)
        //                   .Include(ud => ud.Department)
        //                   .Where(ud => ud.IsActive)
        //                   .AsNoTracking()
        //                   .AsQueryable();

        //    if (personId != Guid.Empty)
        //    {
        //        query = query.Where(ud => ud.PersonId == personId);
        //    }

        //    var userDepartmentDtos = await query
        //       .OrderBy(ud => ud.Department.DepartmentName) 
        //       .Select(ud => new Department
        //       {
        //           DepartmentId = (Guid)(ud.DepartmentId != null ? ud.DepartmentId : Guid.Empty),
        //           DepartmentCode = ud.Department != null ? ud.Department.DepartmentCode : string.Empty,
        //           DepartmentName = ud.Department != null ? ud.Department.DepartmentName : string.Empty,
        //           IsActive = ud.IsActive,
        //       })
        //       .ToListAsync(cancellationToken);

        //  return userDepartmentDtos!;
        //}


        public async Task<PaginationHandler<UserDepartmentAssignedDto>> GetAllWithPagingUserOnUserDepartmentAsync(
         int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default)
        {

            var baseQuery = _context.UserDepartment
             .AsNoTracking()
             .Include(ud => ud.UserAccount)
             .ThenInclude(ud => ud.PersonAsUserAccount)
             .Include(ud => ud.Department)
             .Where(ud => ud.UserAccount != null);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string lowerKeyword = keyword.ToLower().Trim();

                baseQuery = baseQuery.Where(ud =>
                    (ud.Department != null && ud.Department.DepartmentName.ToLower().Contains(lowerKeyword)) ||
                    (ud.UserAccount.PersonAsUserAccount.Firstname != null && ud.UserAccount.PersonAsUserAccount.Firstname.ToLower().Contains(lowerKeyword)) ||
                    (ud.UserAccount.PersonAsUserAccount.Middlename != null && ud.UserAccount.PersonAsUserAccount.Middlename.ToLower().Contains(lowerKeyword)) ||
                    (ud.UserAccount.PersonAsUserAccount.Lastname != null && ud.UserAccount.PersonAsUserAccount.Lastname.ToLower().Contains(lowerKeyword))
                );
            }

            var groupedQuery = baseQuery.GroupBy(ud => ud.UserAccountId);
            int totalRecords = await groupedQuery.CountAsync(cancellationToken);

            var userDepartmentAssignedDtos = groupedQuery
                .Select(group => new UserDepartmentAssignedDto
                {
                    userAccountId = group.Key,
                    PersonId = group.FirstOrDefault().UserAccount.PersonId,
                    UserDepartmentId = group.FirstOrDefault().UserDepartmentId,
                    Firstname = group.FirstOrDefault().UserAccount.PersonAsUserAccount.Firstname ?? string.Empty,
                    Middlename = group.FirstOrDefault().UserAccount.PersonAsUserAccount.Middlename ?? string.Empty,
                    Lastname = group.FirstOrDefault().UserAccount.PersonAsUserAccount.Lastname ?? string.Empty,
                    Username = group.FirstOrDefault().UserAccount.Username,
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


        public async Task<PaginationHandler<UserDepartmentAssignedDto>> GetAllPersonUserAccountPagedAsync(int pageNo, int facilityId, int limit, string? keyword = "", CancellationToken cancellationToken = default)
        {
            string loweredKeyword = keyword != null ? keyword.ToLower() : "";

            var query = _context.Set<UserAccount>()
                .Include(i => i.PersonAsUserAccount)
                .AsNoTracking(); // Good for read-only scenarios

            if (facilityId != 0)
            {
                // This filter is applied only if a specific facility is requested.
                query = query.Where(i => i.FacilityId == facilityId);
            }

            if (!string.IsNullOrWhiteSpace(loweredKeyword))
            {
                query = query.Where(i => i.PersonAsUserAccount.Fullname.ToLower().Contains(loweredKeyword));
            }

            int totalRecords = await query.CountAsync();

            var accountList = query.Select(i => new UserDepartmentAssignedDto
            {
               PersonId = i.PersonId,
               userAccountId = i.UserAccountId,
               Username = i.Username,
               Firstname = i.PersonAsUserAccount.Firstname,
               Middlename = i.PersonAsUserAccount.Middlename,
               Lastname = i.PersonAsUserAccount.Lastname,
            }
            ).ToList();

            var paginatedResult = new PaginationHandler<UserDepartmentAssignedDto>(accountList, totalRecords, pageNo, limit);
            return paginatedResult;
        }

        public async Task<PaginationHandler<Department>> GetDepartmentsExcludedAsync(
            Guid personId,
            int pageNo,
            int limit,
            string? keyword,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Department> query = _context.Set<Department>().AsNoTracking();

            List<Guid> departmentIds = await _context.UserDepartment
            .Where(ud => (ud.UserAccount.PersonId == personId))
            .Select(ud => ud.DepartmentId.Value)
            .ToListAsync(cancellationToken);

            if (departmentIds.Count > 0)
            {
                query = query.Where(i => (!departmentIds.Contains(i.DepartmentId)));
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(d => d.DepartmentName.ToLower().Contains(keyword.ToLower()));
            }

            int totalRecords = await query.CountAsync(cancellationToken);

            var departments = await query
                .OrderBy(d => d.DepartmentName)
                .Skip((pageNo - 1) * limit)
                .Take(limit)
                .ToListAsync(cancellationToken);

            var paginatedResult = new PaginationHandler<Department>(departments, totalRecords, pageNo, limit);
            return paginatedResult;
        }
    }
}
