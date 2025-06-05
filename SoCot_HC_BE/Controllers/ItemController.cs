using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Services;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ItemController : Controller
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet("GetItem/{id}")]
        public async Task<IActionResult> GetItem(Guid id, CancellationToken cancellationToken)
        {
            var item = await _itemService.GetAsync(id, cancellationToken);
            if (item == null)
            {
                return NotFound(new { success = false, message = "Item not found." });
            }

            return Ok(item);

        }

        [HttpGet("GetPagedItems")]
        public async Task<IActionResult> GetPagedItems(int pageNo, int statusId, [FromQuery] List<Guid>? itemCategories, int limit, CancellationToken cancellationToken, string keyword = "")
        {
            if (pageNo <= 0 || limit <= 0)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }

            var paginatedResult = await _itemService.GetAllWithPagingAsync(pageNo, statusId, itemCategories, limit, keyword, cancellationToken);
            return Ok(paginatedResult);
        }

        [HttpPost("SaveItem")]
        public async Task<IActionResult> SaveItem(ItemDTO item, CancellationToken cancellationToken)
        {
            try
            {
                await _itemService.SaveItemAsync(item, cancellationToken);

                return Ok(new
                {
                    success = true,
                    message = item.ItemId == Guid.Empty
                        ? "Item created successfully."
                        : "Item updated successfully."
                });
            }
            catch (ModelValidationException ex)
            {
                foreach (var kvp in ex.Errors)
                {
                    foreach (var error in kvp.Value)
                    {
                        ModelState.AddModelError(kvp.Key, error);
                    }
                }

                var modelErrors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToList()
                );

                return BadRequest(new { success = false, errors = modelErrors });
            }
        }
    }
}
