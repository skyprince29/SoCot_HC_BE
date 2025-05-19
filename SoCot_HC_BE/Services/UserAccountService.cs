using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SCHC_API.Handler;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Helpers;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;
using System.Linq.Expressions;
using System.Threading;

namespace SoCot_HC_BE.Services
{
    public class UserAccountService : Repository<UserAccount, Guid>, IUserAccountService
    {
        public UserAccountService(AppDbContext context) : base(context)
        {
        }


        public async Task<PaginationHandler<UserAccount>> GetAllWithPagingAsync(int pageNo, int statusId, int facilityId, int userGroupId, int limit, string keyword = "", CancellationToken cancellationToken = default)
        {
            string loweredKeyword = keyword.Any() ? keyword.ToLower() : "";

            int totalRecords = await _dbSet
                .CountAsync(u =>
                    (statusId == 0 || (statusId == 1 && u.IsActive) || (statusId == 2 && !u.IsActive)) &&
                    (
                        u.PersonAsUserAccount != null &&
                        u.PersonAsUserAccount.Fullname.ToLower().Contains(loweredKeyword)
                    ) &&
                    (facilityId == 0 ? u.FacilityId != facilityId : u.FacilityId == facilityId) &&
                    (userGroupId == 0 ? u.UserGroupId != userGroupId : u.UserGroupId == userGroupId)

                );


            var userAccounts = await _dbSet
                            .Include(u => u.PersonAsUserAccount)
                            .Include(u => u.FacilityAsUserAccount)
                            .Include(u => u.UserGroupAsUserAccount)
                            .Where(u =>
                                (statusId == 0 || (statusId == 1 && u.IsActive) || (statusId == 2 && !u.IsActive)) &&
                                (
                                    u.PersonAsUserAccount != null &&
                                    u.PersonAsUserAccount.Fullname.ToLower().Contains(loweredKeyword)
                                ) &&
                                (facilityId == 0 ? u.FacilityId != facilityId : u.FacilityId == facilityId) &&
                                (userGroupId == 0 ? u.UserGroupId != userGroupId : u.UserGroupId == userGroupId)
                            )
                            .AsNoTracking()
                            .ToListAsync();

            var paginatedResult = new PaginationHandler<UserAccount>(userAccounts, totalRecords, pageNo, limit);
            return paginatedResult;
        }


        public async Task SaveUserAcccountAsync(UserAccountDTO userAccount, CancellationToken cancellationToken = default)
        {

            // 🔍 Validate fields after DepartmentCode is generated
            ValidateFields(userAccount);

            bool isNew = userAccount.UserAccountId == Guid.Empty || userAccount.UserAccountId == null;
            var account = new UserAccount
            {
                Username = userAccount.Username,
                Password = PasswordHelper.HashPassword(userAccount.Password),
                PersonId = userAccount.PersonId,
                FacilityId = (int)userAccount.FacilityId,
                UserGroupId = userAccount.UserGroupId,
                //CreatedBy = Guid.Parse("5170D519-D6FB-440E-F314-08DD937B4512"),
                //CreatedDate = DateTime.UtcNow,
                //UpdatedBy = Guid.Parse("5170D519-D6FB-440E-F314-08DD937B4512"),
                //UpdatedDate = DateTime.UtcNow,
            };
            if (isNew)
            {
                await AddAsync(account, cancellationToken);
            } else
            {
                var existingUserAccount = await _dbSet
                                     .FirstOrDefaultAsync(d => d.UserAccountId == userAccount.UserAccountId, cancellationToken);

                if (existingUserAccount == null)
                    throw new Exception("Department not found.");
      
                _context.Entry(existingUserAccount).CurrentValues.SetValues(userAccount);
                existingUserAccount.Password = PasswordHelper.HashPassword(userAccount.Password);
                await UpdateAsync(existingUserAccount, cancellationToken);
            }
        }

        private void ValidateFields(UserAccountDTO userAccountDTO) {
            var errors = new Dictionary<string, List<string>>();
            if (userAccountDTO.PersonId == Guid.Empty || userAccountDTO.PersonId == null)
            {
                ValidationHelper.AddError(errors, nameof(userAccountDTO.PersonId), "Person is required field");
            } else if (_dbSet.Any(u => u.PersonId == userAccountDTO.PersonId && u.UserAccountId != userAccountDTO.UserAccountId))
            {
                ValidationHelper.AddError(errors, nameof(userAccountDTO.PersonId), "Selected person has already existing account");
            }

            if (userAccountDTO.Username == null || !userAccountDTO.Username.Any())
            {
                ValidationHelper.AddError(errors, nameof(userAccountDTO.Username), "Username is required field");
            } else if (userAccountDTO.Username.Count() > 50)
            {
                ValidationHelper.AddError(errors, nameof(userAccountDTO.Username), "Username is should less than to 50 characters");
            }

            if (userAccountDTO.Password == null || !userAccountDTO.Password.Any())
            {
                ValidationHelper.AddError(errors, nameof(userAccountDTO.Password), "Password is required field");
            } else if (userAccountDTO.Password.Count() > 100)
            {
                ValidationHelper.AddError(errors, nameof(userAccountDTO.Password), "Password is should less than to 100 characters");
            }

            if (userAccountDTO.FacilityId == null || userAccountDTO.FacilityId == 0)
            {
                ValidationHelper.AddError(errors, nameof(userAccountDTO.FacilityId), "Facility is required field");
            }

            if (userAccountDTO.UserGroupId == 0)
            {
                ValidationHelper.AddError(errors, nameof(userAccountDTO.UserGroupId), "User group is required field");
            }
            if (errors.Any())
                 throw new ModelValidationException("Validation failed", errors);
        }

        public override async Task<UserAccount?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
           var userAccount = await _dbSet
                .Include(p => p.PersonAsUserAccount)
                .FirstOrDefaultAsync(f => f.UserAccountId == id, cancellationToken);
            return userAccount;
        }
    }
}
