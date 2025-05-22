using Microsoft.AspNetCore.Mvc;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Services.Interfaces;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SubCategoryController : Controller
    {
        private readonly ISubCategoryService _subCategoryService;

        public SubCategoryController(ISubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }

        // Get a specific service classification by ID
        [HttpGet("GetSubCategory/{id}")]
        public async Task<IActionResult> GetSubCategory(Guid id, CancellationToken cancellationToken)
        {
            var serviceClassification = await _subCategoryService.GetAsync(id, cancellationToken);
            if (serviceClassification == null)
            {
                return NotFound(new { success = false, message = "Item Category not found." });
            }

            return Ok(serviceClassification);
        }

        [HttpGet("GetSubCategories")]
        public async Task<IActionResult> GetSubCategories(
        [FromQuery] bool isActiveOnly = true,
        [FromQuery] Guid? currentId = null,
        CancellationToken cancellationToken = default
        )
        {
            IEnumerable<SubCategory> items;

            if (isActiveOnly && currentId.HasValue && currentId.Value != Guid.Empty)
            {
                items = await _subCategoryService.GetAllActiveWithCurrentAsync(currentId.Value, cancellationToken);
            }
            else if (isActiveOnly)
            {
                items = await _subCategoryService.GetAllActiveOnlyAsync(cancellationToken);
            }
            else
            {
                items = await _subCategoryService.GetAllAsync(cancellationToken);
            }

            return Ok(items);
        }
    }
}
