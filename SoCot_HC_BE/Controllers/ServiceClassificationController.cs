using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceClassification(int id, CancellationToken cancellationToken)
        {
            var serviceClassification = await _serviceClassificationService.GetAsync(id, cancellationToken);
            if (serviceClassification == null)
            {
                return NotFound(new { success = false, message = "Service Classification not found." });
            }

            return Ok(serviceClassification);
        }

        // Get all active service classifications
        [HttpGet("allActive")]
        public async Task<IActionResult> GetAllActive(CancellationToken cancellationToken)
        {
            var items = await _serviceClassificationService.GetAllActiveOnlyAsync(cancellationToken);
            return Ok(items);
        }

        // Get active service classifications including a specific item
        [HttpGet("allActive/{currentId}")]
        public async Task<IActionResult> GetAllActiveWithCurrent(int currentId, CancellationToken cancellationToken)
        {
            var items = await _serviceClassificationService.GetAllActiveWithCurrentAsync(currentId, cancellationToken);
            return Ok(items);
        }
    }
} 