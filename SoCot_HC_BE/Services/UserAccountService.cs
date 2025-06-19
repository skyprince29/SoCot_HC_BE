using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SCHC_API.Handler;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Helpers;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Persons.Interfaces;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SoCot_HC_BE.Services
{
    public class UserAccountService : Repository<UserAccount, Guid>, IUserAccountService
    {
        private readonly IJwtService _jwtService;
        private readonly IPersonService _personService;
        private readonly IAddressService _addressService;
        public UserAccountService(AppDbContext context, 
            IJwtService jwtService,
            IPersonService personService,
            IAddressService addressService
            ) : base(context)
        {
            _jwtService = jwtService;
            _personService = personService;
            _addressService = addressService;
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

        public async Task<UserAccountTokenDTO> VerifyAccount(LoginDTO loginDTO, CancellationToken cancellationToken = default)
        {

            var errors = new Dictionary<string, List<string>>();
            if (loginDTO == null)
            {
                ValidationHelper.AddError(errors, nameof(loginDTO.Password), "Password is required field");
            } else
            {

                if (loginDTO.Username == null)
                {
                    ValidationHelper.AddError(errors, nameof(loginDTO.Username), "Username is required field");
                }

                if (loginDTO.Password == null)
                {
                    ValidationHelper.AddError(errors, nameof(loginDTO.Password), "Password is required field");
                }

                if (loginDTO.Username != null && loginDTO.Password != null)
                {
                    try
                    {
                        var account = await _dbSet
                                .Include(p => p.PersonAsUserAccount)
                                .ThenInclude(a => a.AddressAsPermanent)
                                .Include(u => u.FacilityAsUserAccount)
                                .Include(u => u.UserGroupAsUserAccount)
                                .FirstOrDefaultAsync(a => a.Username == loginDTO.Username, cancellationToken);

                        if (account != null)
                        {

                            bool isPasswordValid = PasswordHelper.VerifyPassword(loginDTO.Password, account.Password);

                            if (!isPasswordValid)
                            {
                                ValidationHelper.AddError(errors, nameof(loginDTO.Password), "Username or Password is incorrect");
                            }
                            else
                            {
                                UserAccountTokenDTO userAccount = new UserAccountTokenDTO();

                                var token = _jwtService.GenerateToken(account.UserAccountId); // assuming Id is Guid

                                return new UserAccountTokenDTO
                                {
                                    userAccount = account,
                                    Token = token
                                };
                            }
                        }
                        else
                        {
                            ValidationHelper.AddError(errors, nameof(loginDTO.Password), "Account not found");
                        }
                    } catch (Exception ex)
                    {
                        Console.WriteLine(ex.InnerException);
                    }
                   
 
                }
            }
            throw new InvalidOperationException("Usernane or Password is incorrect");
        }

        public async Task<List<UserCsvDto>> UploadCsv(IFormFile file, CancellationToken cancellationToken = default)
        {
            var users = new List<UserCsvDto>();
            var failedUser = new List<string>();
            var logFileName = $"FailedUserSaves_{DateTime.UtcNow:yyyyMMddHHmmss}.txt";
            var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CSVFileLogs", logFileName);
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath)!);
            Person person = null;
            try
            {
                using var reader = new StreamReader(file.OpenReadStream());

                string? headerLine = await reader.ReadLineAsync(); // Skip header
                while (!reader.EndOfStream)
                {
                    string? line = await reader.ReadLineAsync();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var values = line.Split(',');
                    if (values.Length < 20) continue;

                    string firstname = values[11];
                    string middlename = values[12];
                    string lastname = values[13];
                    string personName = $"{firstname} {lastname}";
                    string Suffix = values[14];
                    string contactNo = values[10];
                    string email = values[9];

                    int provinceId = int.TryParse(values[16], out var provId) ? provId : 0;
                    int municipalCityId = int.TryParse(values[17], out var cityId) ? cityId : 0;
                    int barangayId = int.TryParse(values[18], out var brgyId) ? brgyId : 0;

                    await using var transaction = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        if (firstname == null && middlename == null && lastname == null)
                        {
                            failedUser.Add($"Failed to save {personName}: Province : {values[15]} City: {values[16]} Barangay : {values[17]}");
                        } else
                        {
                            var address = new Address
                            {
                                ProvinceId = provinceId,
                                MunicipalityId = municipalCityId,
                                BarangayId = barangayId
                            };
                            await _context.Address.AddAsync(address);
                            await _context.SaveChangesAsync(); // Save to get AddressId

                            person = new Person
                            {
                                Firstname = firstname,
                                Middlename = middlename,
                                Lastname = lastname,
                                Suffix = Suffix,
                                BirthDate = DateTime.Now, // Replace with parsed value if needed
                                Gender = "",
                                CivilStatus = "",
                                Religion = "",
                                ContactNo = values[9],
                                Email = values[8],
                                AddressIdResidential = address.AddressId,
                                AddressIdPermanent = address.AddressId,
                                IsDeceased = false,
                                Citizenship = "Filipino",
                                BloodType = "N/A",
                                PatientIdTemp = 0,
                                CreatedDate = DateTime.Now,
                                UpdatedDate = DateTime.Now
                            };
                            await _context.Person.AddAsync(person);
                            await _context.SaveChangesAsync();

                            //UserAccount userAccount = new UserAccount()
                            //{

                            //};
                            await transaction.CommitAsync();
                        }

                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        failedUser.Add($"Failed to save {personName}: for unknown error {person}");
                    }
                }

                // Write all failed users at the end
                if (failedUser.Any())
                {
                    await File.WriteAllLinesAsync(logFilePath, failedUser);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while uploading CSV", ex);
            }

            return users;
        }

    }
}
