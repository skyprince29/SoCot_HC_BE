using Microsoft.AspNetCore.Mvc;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Services.Interfaces;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentTypeController : Controller
    {
        private readonly IDepartmentTypeService _departmentTypeService;

        public DepartmentTypeController(IDepartmentTypeService departmentTypeService)
        {
            _departmentTypeService = departmentTypeService;
        }

        // Get a specific service classification by ID
        [HttpGet("GetDepartmentType/{id}")]
        public async Task<IActionResult> GetDepartmentType(Guid id, CancellationToken cancellationToken)
        {
            var serviceClassification = await _departmentTypeService.GetAsync(id, cancellationToken);
            if (serviceClassification == null)
            {
                return NotFound(new { success = false, message = "Department Type not found." });
            }

            return Ok(serviceClassification);
        }

        [HttpGet("GetDepartmentTypes")]
        public async Task<IActionResult> GetDepartmentTypes(
        [FromQuery] bool isActiveOnly = true,
        [FromQuery] Guid? currentId = null,
        CancellationToken cancellationToken = default
        )
        {
            IEnumerable<DepartmentType> items;

            if (isActiveOnly && currentId.HasValue && currentId.Value != Guid.Empty)
            {
                items = await _departmentTypeService.GetAllActiveWithCurrentAsync(currentId.Value, cancellationToken);
            }
            else if (isActiveOnly)
            {
                items = await _departmentTypeService.GetAllActiveOnlyAsync(cancellationToken);
            }
            else
            {
                items = await _departmentTypeService.GetAllAsync(cancellationToken);
            }

            return Ok(items);
        }
    }
}
