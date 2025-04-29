using Microsoft.AspNetCore.Mvc;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Services.Interfaces;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class WoundTypeController : Controller
    {
        private readonly IWoundTypeService _woundTypeService;

        public WoundTypeController(IWoundTypeService woundTypeService)
        {
            _woundTypeService = woundTypeService;
        }

        [HttpGet("GetWoundType/{id}")]
        public async Task<IActionResult> GetWoundType(int id, CancellationToken cancellationToken)
        {
            var serviceClassification = await _woundTypeService.GetAsync(id, cancellationToken);
            if (serviceClassification == null)
            {
                return NotFound(new { success = false, message = "Wound Type not found." });
            }

            return Ok(serviceClassification);
        }

        [HttpGet("GetWoundTypes")]
        public async Task<IActionResult> GetWoundTypes(
        [FromQuery] bool isActiveOnly = true,
        [FromQuery] int? currentId = null,
        CancellationToken cancellationToken = default
        )
        {
            IEnumerable<WoundType> items;

            if (isActiveOnly && currentId.HasValue && currentId.Value > 0)
            {
                items = await _woundTypeService.GetAllActiveWithCurrentAsync(currentId.Value, cancellationToken);
            }
            else if (isActiveOnly)
            {
                items = await _woundTypeService.GetAllActiveOnlyAsync(cancellationToken);
            }
            else
            {
                items = await _woundTypeService.GetAllAsync(cancellationToken);
            }

            return Ok(items);
        }
    }
}
