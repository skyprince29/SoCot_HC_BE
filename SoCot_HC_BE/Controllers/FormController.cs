using Microsoft.AspNetCore.Mvc;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Services.Interfaces;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FormController : Controller
    {
        private readonly IFormService _formService;

        public FormController(IFormService formService)
        {
            _formService = formService;
        }

        // Get a specific service classification by ID
        [HttpGet("GetForm/{id}")]
        public async Task<IActionResult> GetForm(Guid id, CancellationToken cancellationToken)
        {
            var serviceClassification = await _formService.GetAsync(id, cancellationToken);
            if (serviceClassification == null)
            {
                return NotFound(new { success = false, message = "Form not found." });
            }

            return Ok(serviceClassification);
        }

        [HttpGet("GetForms")]
        public async Task<IActionResult> GetForms(
        [FromQuery] bool isActiveOnly = true,
        [FromQuery] Guid? currentId = null,
        CancellationToken cancellationToken = default
        )
        {
            IEnumerable<Form> items;

            if (isActiveOnly && currentId.HasValue && currentId.Value != Guid.Empty)
            {
                items = await _formService.GetAllActiveWithCurrentAsync(currentId.Value, cancellationToken);
            }
            else if (isActiveOnly)
            {
                items = await _formService.GetAllActiveOnlyAsync(cancellationToken);
            }
            else
            {
                items = await _formService.GetAllAsync(cancellationToken);
            }

            return Ok(items);
        }
    }
}
