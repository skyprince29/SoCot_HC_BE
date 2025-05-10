using Microsoft.AspNetCore.Mvc;
using SoCot_HC_BE.DTO.OldReferralDto;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Services.Interfaces;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReferralController : Controller
    {
        private readonly IReferralService _referralService;

        public ReferralController(IReferralService referralService)
        {
            _referralService = referralService;
        }   

        [HttpGet("GetUHCReferralByIdandFacilityId")]
        public async Task<IActionResult> GetUHCReferralByIdandFacilityId(int referralId, int facilityId, CancellationToken cancellationToken)
        {
            try
            {
                UHCReferralDTO dto = await _referralService.GetUHCReferralAsync(referralId, facilityId, cancellationToken);
                return Ok(dto);
            }
            catch (Exception ex) {
                return BadRequest(new { message = "Request has been interupted" });
            }

        }
    }
}
