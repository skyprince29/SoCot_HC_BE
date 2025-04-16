using Microsoft.AspNetCore.Mvc;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Services.Interfaces;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ServiceClassificationController : Controller
    {
        private readonly IServiceClassificationService _serviceClassificationService;

        public ServiceClassificationController(IServiceClassificationService serviceClassificationService)
        {
            _serviceClassificationService = serviceClassificationService;
        }

        // Get a specific service classification by ID
        [HttpGet("GetServiceClassification/{id}")]
        public async Task<IActionResult> GetServiceClassification(int id, CancellationToken cancellationToken)
        {
            var serviceClassification = await _serviceClassificationService.GetAsync(id, cancellationToken);
            if (serviceClassification == null)
            {
                return NotFound(new { success = false, message = "Service Classification not found." });
            }

            return Ok(serviceClassification);
        }

        [HttpGet("GetServiceClassifications")]
        public async Task<IActionResult> GetServiceClassifications(
        [FromQuery] bool isActiveOnly = true,
        [FromQuery] int? currentId = null,
        CancellationToken cancellationToken = default
        )
        {
            IEnumerable<ServiceClassification> items;

            if (isActiveOnly && currentId.HasValue && currentId.Value > 0)
            {
                items = await _serviceClassificationService.GetAllActiveWithCurrentAsync(currentId.Value, cancellationToken);
            }
            else if (isActiveOnly)
            {
                items = await _serviceClassificationService.GetAllActiveOnlyAsync(cancellationToken);
            }
            else
            {
                items = await _serviceClassificationService.GetAllAsync(cancellationToken);
            }

            return Ok(items);
        }
    }
} 