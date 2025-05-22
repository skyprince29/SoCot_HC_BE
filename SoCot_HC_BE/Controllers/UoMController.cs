using Microsoft.AspNetCore.Mvc;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Services.Interfaces;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UoMController : Controller
    {
        private readonly IUoMService _uoMService;

        public UoMController(IUoMService uoMService)
        {
            _uoMService = uoMService;
        }

        // Get a specific service classification by ID
        [HttpGet("GetUoM/{id}")]
        public async Task<IActionResult> GetUoM(Guid id, CancellationToken cancellationToken)
        {
            var serviceClassification = await _uoMService.GetAsync(id, cancellationToken);
            if (serviceClassification == null)
            {
                return NotFound(new { success = false, message = "UoM not found." });
            }

            return Ok(serviceClassification);
        }

        [HttpGet("GetUoMs")]
        public async Task<IActionResult> GetUoMs(
        [FromQuery] bool isActiveOnly = true,
        [FromQuery] Guid? currentId = null,
        CancellationToken cancellationToken = default
        )
        {
            IEnumerable<UoM> items;

            if (isActiveOnly && currentId.HasValue && currentId.Value != Guid.Empty)
            {
                items = await _uoMService.GetAllActiveWithCurrentAsync(currentId.Value, cancellationToken);
            }
            else if (isActiveOnly)
            {
                items = await _uoMService.GetAllActiveOnlyAsync(cancellationToken);
            }
            else
            {
                items = await _uoMService.GetAllAsync(cancellationToken);
            }

            return Ok(items);
        }
    }
}
