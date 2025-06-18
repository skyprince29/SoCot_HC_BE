using Microsoft.AspNetCore.Mvc;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Services.Interfaces;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FamilyController : ControllerBase
    {

        private readonly IFamilyService _familyService;
        public FamilyController(IFamilyService familyService)
        {
            _familyService = familyService;
        }


        [HttpPost("SaveFamily")]
        public async Task<IActionResult> SaveFamily(FamilyDto familyDto, CancellationToken cancellationToken = default)
        {
            try
            {
                await _familyService.SaveFamily(familyDto, cancellationToken);
                string msg = familyDto.Id == Guid.Empty ? "Family saved successfully" : "Family update successfully";
                return Ok(new { success = true, message = msg });
            }
            catch
            {
                return BadRequest(new { success = false, message = "An error occur during execution please contact your administrator" });
            }
          
        }
    }
}
