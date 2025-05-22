using Microsoft.AspNetCore.Mvc;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Services.Interfaces;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ItemCategoryController : Controller
    {
        private readonly IItemCategoryService _itemCategoryService;

        public ItemCategoryController(IItemCategoryService itemCategoryService)
        {
            _itemCategoryService = itemCategoryService;
        }

        // Get a specific service classification by ID
        [HttpGet("GetItemCategory/{id}")]
        public async Task<IActionResult> GetItemCategory(Guid id, CancellationToken cancellationToken)
        {
            var serviceClassification = await _itemCategoryService.GetAsync(id, cancellationToken);
            if (serviceClassification == null)
            {
                return NotFound(new { success = false, message = "Item Category not found." });
            }

            return Ok(serviceClassification);
        }

        [HttpGet("GetItemCategories")]
        public async Task<IActionResult> GetItemCategories(
        [FromQuery] bool isActiveOnly = true,
        [FromQuery] Guid? currentId = null,
        CancellationToken cancellationToken = default
        )
        {
            IEnumerable<ItemCategory> items;

            if (isActiveOnly && currentId.HasValue && currentId.Value != Guid.Empty)
            {
                items = await _itemCategoryService.GetAllActiveWithCurrentAsync(currentId.Value, cancellationToken);
            }
            else if (isActiveOnly)
            {
                items = await _itemCategoryService.GetAllActiveOnlyAsync(cancellationToken);
            }
            else
            {
                items = await _itemCategoryService.GetAllAsync(cancellationToken);
            }

            return Ok(items);
        }
    }
}
