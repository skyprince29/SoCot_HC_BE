using Microsoft.AspNetCore.Mvc;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Services.Interfaces;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StrengthController : Controller
    {
        private readonly IStrengthService _strengthService;

        public StrengthController(IStrengthService strengthService)
        {
            _strengthService = strengthService;
        }

        // Get a specific service classification by ID
        [HttpGet("GetStrength/{id}")]
        public async Task<IActionResult> GetStrength(Guid id, CancellationToken cancellationToken)
        {
            var serviceClassification = await _strengthService.GetAsync(id, cancellationToken);
            if (serviceClassification == null)
            {
                return NotFound(new { success = false, message = "Strength not found." });
            }

            return Ok(serviceClassification);
        }

        [HttpGet("GetStrengths")]
        public async Task<IActionResult> GetStrengths(
        [FromQuery] bool isActiveOnly = true,
        [FromQuery] Guid? currentId = null,
        CancellationToken cancellationToken = default
        )
        {
            IEnumerable<Strength> items;

            if (isActiveOnly && currentId.HasValue && currentId.Value != Guid.Empty)
            {
                items = await _strengthService.GetAllActiveWithCurrentAsync(currentId.Value, cancellationToken);
            }
            else if (isActiveOnly)
            {
                items = await _strengthService.GetAllActiveOnlyAsync(cancellationToken);
            }
            else
            {
                items = await _strengthService.GetAllAsync(cancellationToken);
            }

            return Ok(items);
        }
    }
}
