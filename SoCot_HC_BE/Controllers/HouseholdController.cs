using Microsoft.AspNetCore.Mvc;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Model.Requests;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HouseholdController : ControllerBase
    {
        private readonly IHouseholdService _householdService;

        public HouseholdController(IHouseholdService householdService)
        {
            _householdService = householdService;
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveHousehold([FromBody] SaveHouseholdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _householdService.SaveHouseholdAsync(request, cancellationToken);

                return Ok(new { success = true, message = "Household saved successfully." });
            }
            catch (ModelValidationException ex)
            {
                return BadRequest(new { success = false, message = "Please fill in all required fields.", errors = ex.Errors });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

    }
}
