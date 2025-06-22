using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO.OldReferralDto;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SoCot_HC_BE.Services
{
    public class ReferralService : Repository<ReferralDto, Guid>, IReferralService
    {
        private readonly HttpClient _httpClient;
        private readonly ExternalApiSettings _apiSettings;
        private readonly IHouseholdService _householdService;
        private readonly IFamilyService _familyService;
        private readonly IFamilyMemberService _familyMemberService;
        private readonly IAddressService _addressService;
        public ReferralService(
         IHouseholdService householdService, 
         IFamilyService familyService, 
         IFamilyMemberService familyMemberService,
         IAddressService addressService,
         AppDbContext context, HttpClient httpClient, IOptions<ExternalApiSettings> options)
         : base(context)
        {
            _httpClient = httpClient;
            _apiSettings = options.Value;
            _httpClient.BaseAddress = new Uri(_apiSettings.BaseUrl);
            _householdService = householdService;
            _familyService = familyService;
            _familyMemberService = familyMemberService;
            _addressService = addressService;
        }

        public async Task<UHCReferralDTO> GetUHCReferralAsync(int referralId, int facilityId, CancellationToken cancellationToken = default)
        {
            UHCReferralDTO? referralDto = new UHCReferralDTO();
            try
            {
                var credentials = new
                {
                    username = _apiSettings.Username,
                    password = _apiSettings.Password
                };
                var content = new StringContent(
                            JsonSerializer.Serialize(credentials),
                            Encoding.UTF8,
                            "application/json");

                var loginRoute = _apiSettings.BaseUrl + "/Login";
                var authResponse = await _httpClient.PostAsync(loginRoute + _apiSettings.AuthEndpoint, content, cancellationToken);

                if (!authResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Authentication failed.");
                }

                string token = await authResponse.Content.ReadAsStringAsync(cancellationToken);

                if (string.IsNullOrWhiteSpace(token))
                {
                    throw new Exception("Token not returned by the authentication service.");
                }
                referralDto = await UHCReferralDetails(token, referralId, facilityId, cancellationToken);

                return referralDto;
            }
               catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new UHCReferralDTO();
            }
        }

        private async Task<UHCReferralDTO> UHCReferralDetails(string token, int referralId, int facilityId, CancellationToken cancellationToken = default)
        {
            UHCReferralDTO? referralDto = new UHCReferralDTO();
            try {
                var requestUrl = $"{_apiSettings.BaseUrl}/GetReferralByRefIdAndFacilityId?ReferralId={referralId}&FacilityId={facilityId}";

                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var referralResponse = await _httpClient.SendAsync(request, cancellationToken);

                if (!referralResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to retrieve referral data.");
                }
                var referralJson = await referralResponse.Content.ReadAsStringAsync(cancellationToken);


                referralDto = JsonSerializer.Deserialize<UHCReferralDTO>(referralJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });


                if (referralDto == null)
                {
                    throw new Exception("Failed to deserialize referral data.");
                }
                return referralDto;
            }
            catch (Exception ex)
            {
                return new UHCReferralDTO();                                                                                           
            }
        }

        public async Task<bool> MigrateReferral(UHCReferralDTO dto)
        {
            try
            {
                if (dto.HouseHold != null)
                {
                    Guid existingHouseholdId = await _context.Households
                                                .Where(h => h.TempHouseholdId == dto.HouseHold.Id)
                                                .Select(h => h.HouseholdId)
                                                .FirstOrDefaultAsync();
                    if (existingHouseholdId == Guid.Empty)
                    {
                        Address address = new Address()
                        {
                            AddressId = Guid.NewGuid(),
                            BarangayId = (int)dto.HouseHold.barangay_id,
                            MunicipalityId = dto.HouseHold.city_municipality_id,
                            ProvinceId = dto.HouseHold.province_id,
                            Sitio = dto.HouseHold.Sitio,
                            Purok = dto.HouseHold.Purok,
                            ZipCode = dto.HouseHold.Zipcode,
                            HouseNo = dto.HouseHold.HouseNo,
                            LotNo = dto.HouseHold.LotNo,
                            BlockNo = dto.HouseHold.BlockNo,
                            Street = dto.HouseHold.Street,
                            Subdivision = dto.HouseHold.Subdivision
                        };
                        await _addressService.AddAsync(address);

                        Household household = new Household()
                        {
                            HouseholdId = new Guid(),
                            HouseholdNo = dto.HouseHold.PHouseholdNo,
                            ResidenceName = dto.HouseHold.ResidenceName,
                            AddressId = address.AddressId,
                            IsActive = dto.HouseHold.IsActive
                            
                        };

                    }

                }

                return true;
            }
            catch (Exception ex) {
                return false;
            }
        }

        public async Task<UHCReferralDTO> MarkReferralArrived(int referralId, int facilityId, CancellationToken cancellationToken = default)
        {
            UHCReferralDTO? referralDto = new UHCReferralDTO();
            try
            {
                var credentials = new
                {
                    username = _apiSettings.Username,
                    password = _apiSettings.Password
                };
                var content = new StringContent(
                            JsonSerializer.Serialize(credentials),
                            Encoding.UTF8,
                            "application/json");

                var loginRoute = _apiSettings.BaseUrl + "/Login";
                var authResponse = await _httpClient.PostAsync(loginRoute + _apiSettings.AuthEndpoint, content, cancellationToken);

                if (!authResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Authentication failed.");
                }

                string token = await authResponse.Content.ReadAsStringAsync(cancellationToken);

                if (string.IsNullOrWhiteSpace(token))
                {
                    throw new Exception("Token not returned by the authentication service.");
                }
                referralDto = await UHCReferralDetails(token, referralId, facilityId, cancellationToken);

                ReferralDto? uhcReferralDTO = referralDto.Referral;
                UHCHouseHoldDto? houseHoldDto = referralDto.HouseHold;
                UHCFamilyDto? uHCFamilyDto = referralDto.Family;
                UHCFamilyMemberDto? familyMemberDto = referralDto.FamilyMember;
                UHCPatientDto? uHCPatientDto = referralDto.Patient;

                await using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    if (houseHoldDto != null)
                    {

                        // Check if address already exist
                        Address? address = await _context.Address.Where(a => a.BarangayId == houseHoldDto.barangay_id
                          && a.MunicipalityId == houseHoldDto.city_municipality_id
                          && a.ProvinceId == houseHoldDto.province_id
                          && a.Sitio == houseHoldDto.Sitio
                          && a.Purok == houseHoldDto.Purok
                          && a.ZipCode == houseHoldDto.Zipcode
                          && a.HouseNo == houseHoldDto.HouseNo
                          && a.LotNo == houseHoldDto.LotNo
                          && a.BlockNo == houseHoldDto.BlockNo
                          && a.Street == houseHoldDto.Street
                          && a.Subdivision == houseHoldDto.Subdivision
                            ).FirstOrDefaultAsync(cancellationToken: cancellationToken);

                        if (address == null)
                        {
                            address = new Address()
                            {
                                BarangayId = houseHoldDto.barangay_id,
                                MunicipalityId = houseHoldDto.city_municipality_id,
                                ProvinceId = houseHoldDto.province_id,
                                Sitio = houseHoldDto.Sitio,
                                Purok = houseHoldDto.Purok,
                                ZipCode = houseHoldDto.Zipcode,
                                HouseNo = houseHoldDto.HouseNo,
                                LotNo = houseHoldDto.LotNo,
                                BlockNo = houseHoldDto.BlockNo,
                                Street = houseHoldDto.Street,
                                Subdivision = houseHoldDto.Subdivision,
                            };
                            await _context.Address.AddAsync(address);
                            await _context.SaveChangesAsync();

                        }

                        // Validate Person
                        if (uHCPatientDto != null)
                        {

                            Person? person = await _context.Person
                                                        .Where(p => p.Firstname == uHCPatientDto.Firstname
                                                        && p.Lastname == uHCPatientDto.Lastname
                                                        && p.BirthDate == uHCPatientDto.Birthdate)
                                                        .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                            // Save new person
                            if (person == null)
                            {
                                person = new Person()
                                {
                                    Firstname = uHCPatientDto.Firstname,
                                    Middlename = uHCPatientDto.Middlename,
                                    Lastname = uHCPatientDto.Lastname,
                                    Suffix = uHCPatientDto.Suffix,
                                    BirthDate = uHCPatientDto.Birthdate,
                                    BirthPlace = uHCPatientDto.BirthPlace,
                                    Gender = uHCPatientDto.Gender,
                                    CivilStatus = uHCPatientDto.CivilStatus,
                                    ContactNo = uHCPatientDto.ContactNumber,
                                    AddressIdResidential = address.AddressId,
                                    AddressIdPermanent = address.AddressId,
                                    IsDeceased = false,
                                    Citizenship = uHCPatientDto.Citizenship,
                                    BloodType = "N/A",
                                    PatientIdTemp = uHCPatientDto.Id
                                };
                                await _context.Person.AddAsync(person);
                                await _context.SaveChangesAsync();
                            }

                            // Save household
                            if (houseHoldDto != null)
                            {
                                Household? existingHousehold = await _context.Households
                                                                .Where(h => h.HouseholdNo == uHCPatientDto.PHouseholdNo)
                                                                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
                                if (existingHousehold == null)
                                {
                                     existingHousehold = new Household()
                                    {
                                        HouseholdNo = houseHoldDto.PHouseholdNo,
                                        ResidenceName = houseHoldDto.ResidenceName,
                                        AddressId = address.AddressId,
                                        IsActive = houseHoldDto.IsActive,
                                    };
                                    await _context.Households.AddAsync(existingHousehold);
                                    await _context.SaveChangesAsync(); 
                                }

                                // Save family
                                if (uHCFamilyDto != null)
                                {
                                    Family? existingFamily = await _context.Families
                                                            .Where(f => f.FamilyNo == uHCFamilyDto.FamilySerialNo)
                                                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                                    if (existingFamily == null)
                                    {
                                        existingFamily = new Family()
                                        {
                                            HouseholdId = existingHousehold.HouseholdId,
                                            PersonId = person.PersonId

                                        };
                                    }
                                }
                            }
                        }
                        else
                        {
                            //throw new Exception("No house hold attached");
                            return new UHCReferralDTO();
                        }
                    }

                    return referralDto;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    //throw new Exception("Something went wrong please report this issue to your system admin");
                    return new UHCReferralDTO();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new UHCReferralDTO();
            }
        }
    }

}
