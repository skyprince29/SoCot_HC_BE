using CsvHelper;
using Microsoft.EntityFrameworkCore;
using SCHC_API.Handler;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.Designations.Interfaces;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Helpers;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Persons.Interfaces;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SoCot_HC_BE.Services
{
    public class UserAccountService : Repository<UserAccount, Guid>, IUserAccountService
    {
        private readonly IJwtService _jwtService;
        private readonly IPersonService _personService;
        private readonly IAddressService _addressService;
        private readonly IDesignationService _designationService;
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

        public async Task<UploadedCSVSummaryDto> UploadCsv(IFormFile file, CancellationToken cancellationToken = default)
        {
            var logFileName = $"FailedUserSaves_{DateTime.UtcNow:yyyyMMddHHmmss}.txt";
            var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CSVFileLogs", logFileName);
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath)!);

            var users = new List<UserCsvDto>();
            int countSaveRecord = 0;
            int countFailedSaveRecord = 0;
            int totalRecord = 0;

            try
            {
                using var reader = new StreamReader(file.OpenReadStream());
                using var csv = new CsvReader(reader, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    BadDataFound = null
                });

                var records = csv.GetRecords<UserCsvDto>().ToList();

                foreach (var record in records)
                {
                    totalRecord++;

                    string fullname = record.Name?.Trim() ?? "";
                    string firstname = record.FirstName?.Trim() ?? "";
                    string lastname = record.LastName?.Trim() ?? "";
                    string middlename = record.MiddleName?.Trim() ?? "";
                    string username = record.Username?.Trim() ?? "";
                    string designationName = record.Designation?.Trim() ?? "";
                    string contactNo = record.ContactNumber?.Trim() ?? "";
                    string email = record.Email?.Trim() ?? "";

                    int facilityId = int.TryParse(record.FacilityId?.Trim(), out var fId) ? fId : 0;
                    int provinceId = int.TryParse(record.ProvinceID?.Trim(), out var provId) ? provId : 0;
                    int municipalCityId = int.TryParse(record.CityMunicipalityID?.Trim(), out var cityId) ? cityId : 0;
                    int barangayId = int.TryParse(record.barangay_id?.Trim(), out var brgyId) ? brgyId : 0;
                    int userIdTemp = int.TryParse(record.Id?.Trim(), out var tempId) ? tempId : 0;

                    if (firstname != string.Empty && lastname != string.Empty)
                    {
                        var existingPerson = await _context.Person
                                            .FirstOrDefaultAsync(p => p.Firstname == firstname && p.Lastname == lastname);

                        if (existingPerson != null)
                        {
                            await File.AppendAllTextAsync(logFilePath, $"{fullname} already save" + Environment.NewLine);
                            continue;
                        }
                    }

                    bool isValidData = await IsValidData(fullname, firstname, lastname, username, provinceId, municipalCityId, barangayId, facilityId, logFilePath);
                    if (!isValidData)
                    {
                        countFailedSaveRecord++;
                        continue;
                    }

                    await using var transaction = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        var address = await _context.Addresses.FirstOrDefaultAsync(d =>
                            d.ProvinceId == provinceId &&
                            d.MunicipalityId == municipalCityId &&
                            d.BarangayId == barangayId &&
                            d.Sitio == null &&
                            d.Purok == null &&
                            d.ZipCode == null &&
                            d.HouseNo == null &&
                            d.BlockNo == null &&
                            d.Street == null &&
                            d.Subdivision == null);

                        if (address == null)
                        {
                            address = new Address
                            {
                                ProvinceId = provinceId,
                                MunicipalityId = municipalCityId,
                                BarangayId = barangayId
                            };
                            await _context.Address.AddAsync(address);
                            await _context.SaveChangesAsync();
                        }

                        var person = new Person
                        {
                            Firstname = firstname,
                            Middlename = middlename,
                            Lastname = lastname,
                            Suffix = record.Suffix?.Trim(),
                            BirthDate = DateTime.Now, // Placeholder
                            Gender = "",
                            CivilStatus = "",
                            Religion = "",
                            ContactNo = contactNo,
                            Email = email,
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

                        int defaultUserGroup = 5;

                        var designation = await _context.Designation.FirstOrDefaultAsync(d => d.DesignationName == designationName);
                        if (designation == null)
                        {
                            designation = new Designation
                            {
                                DesignationCode = "",
                                DesignationName = designationName
                            };
                            await _context.Designation.AddAsync(designation);
                            await _context.SaveChangesAsync();
                        }

                        var userAccount = new UserAccount
                        {
                            Username = username,
                            Password = PasswordHelper.HashPassword("x"),
                            PersonId = person.PersonId,
                            FacilityId = facilityId,
                            UserGroupId = defaultUserGroup,
                            RememberMeToken = "",
                            IsOnline = false,
                            IsinitLogin = false,
                            IsActive = true,
                            UserIdTemp = userIdTemp,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            DesignationId = designation.DesignationId
                        };

                        await _context.UserAccount.AddAsync(userAccount);
                        await _context.SaveChangesAsync();

                        //users.Add(new UserCsvDto
                        //{
                        //    FirstName = firstname,
                        //    MiddleName = middlename,
                        //    LastName = lastname,
                        //    Email = email,
                        //    Username = username
                        //});

                        countSaveRecord++;
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        countFailedSaveRecord++;
                        await transaction.RollbackAsync();
                        string error = $"Exception while saving {fullname}: {ex.InnerException?.Message ?? ex.Message}";
                        await File.AppendAllTextAsync(logFilePath, error + Environment.NewLine);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while uploading CSV", ex);
            }

            return new UploadedCSVSummaryDto
            {
                TotalSaved = countSaveRecord,
                TotalFailed = countFailedSaveRecord,
                TotalRecords = totalRecord
            };
        }


        private async Task<bool> IsValidData(string fullName, string firstname, string lastname, string username,
            int provinceId, int municipalityId, int barangayId, int FacilityId, string logFilePath)
        {
            bool isValid = true;
            var errorMessages = new List<string>();
            string errorMsg = $"Failed to save {fullName} with the following error(s): ";
            errorMessages.Add($"Facility {FacilityId}.");
            if (string.IsNullOrWhiteSpace(firstname))
            {
                errorMessages.Add("First name is missing.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(lastname))
            {
                errorMessages.Add("Last name is missing.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                errorMessages.Add("Username is missing.");
                isValid = false;
            }

            if (provinceId == 0)
            {
                errorMessages.Add("Province is missing.");
                isValid = false;
            }

            if (municipalityId == 0)
            {
                errorMessages.Add("Municipality is missing.");
                isValid = false;
            }

            if (barangayId == 0)
            {
                errorMessages.Add("Barangay is missing.");
                isValid = false;
            }

            if (FacilityId == 0)
            {
                errorMessages.Add("Facility is missing.");
                isValid = false;
            }

            if (FacilityId == 0 || !await _context.Facility.AnyAsync(f => f.FacilityId == FacilityId))
            {
                await File.AppendAllTextAsync(logFilePath, $"Invalid FacilityId {FacilityId}");
                isValid = false;
            }

            if (!isValid)
            {
                errorMsg += string.Join(" ", errorMessages);
                await File.AppendAllTextAsync(logFilePath, errorMsg + Environment.NewLine);
            }

            return isValid;
        }
    }
}
