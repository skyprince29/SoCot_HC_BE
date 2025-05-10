using Microsoft.Extensions.Options;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.DTO.OldReferralDto;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

namespace SoCot_HC_BE.Services
{
    public class ReferralService : Repository<ReferralDto, Guid>, IReferralService
    {
        private readonly HttpClient _httpClient;
        private readonly ExternalApiSettings _apiSettings;
        public ReferralService(AppDbContext context, HttpClient httpClient, IOptions<ExternalApiSettings> options)
         : base(context)
        {
            _httpClient = httpClient;
            _apiSettings = options.Value;
            _httpClient.BaseAddress = new Uri(_apiSettings.BaseUrl);
        }

        public async Task<UHCReferralDTO> GetUHCReferralAsync(int referralId, int facilityId, CancellationToken cancellationToken = default)
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


            var authResponse = await _httpClient.PostAsync(_apiSettings.BaseUrl + _apiSettings.AuthEndpoint, content, cancellationToken);

            if (!authResponse.IsSuccessStatusCode)
            {
                throw new Exception("Authentication failed.");
            }

            string token = await authResponse.Content.ReadAsStringAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new Exception("Token not returned by the authentication service.");
            }

            // Step 2: Use token to request referral
            var requestUrl = $"{_apiSettings.BaseUrl}/GetReferralByRefIdAndFacilityId?ReferralId={referralId}&FacilityId={facilityId}";

            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var referralResponse = await _httpClient.SendAsync(request, cancellationToken);

            if (!referralResponse.IsSuccessStatusCode)
            {
                throw new Exception("Failed to retrieve referral data.");
            }
            var referralJson = await referralResponse.Content.ReadAsStringAsync(cancellationToken);

            var referralDto = JsonSerializer.Deserialize<UHCReferralDTO>(referralJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (referralDto == null)
            {
                throw new Exception("Failed to deserialize referral data.");
            }
            return referralDto;
        }
    }
}
