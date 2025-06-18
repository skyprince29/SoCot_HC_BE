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
    }

}
