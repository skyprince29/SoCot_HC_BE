using Microsoft.AspNetCore.Mvc;
using SCHC_API.Handler;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Utils;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.DTO;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class WRAController : Controller
    {
        private readonly IWRAService _wraService;

        public WRAController(IWRAService wraService)
        {
            _wraService = wraService;
        }

        // Get a specific WRA by ID
        [HttpGet("GetWRA/{id}")]
        public async Task<IActionResult> GetWRA(Guid id, CancellationToken cancellationToken)
        {
            var wra = await _wraService.GetAsync(id, cancellationToken);
            if (wra == null)
            {
                return NotFound(new { success = false, message = "WRA not found." });
            }

            return Ok(wra);
        }

        // Get all WRA with paging
        [HttpGet("GetPagedWRAs")]
        public async Task<IActionResult> GetPagedWRAs(int pageNo, int limit, string? keyword, CancellationToken cancellationToken)
        {
            if (pageNo <= 0 || limit <= 0)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }

            var wra = await _wraService.GetAllWithPagingAsync(pageNo, limit, keyword, cancellationToken);
            var totalRecords = await _wraService.CountAsync(keyword, cancellationToken);

            var paginatedResult = new PaginationHandler<WRA>(wra, totalRecords, pageNo, limit);
            return Ok(paginatedResult);
        }

        [HttpPost("SaveWRA")]
        public async Task<IActionResult> SaveWRA(WRADto wra, CancellationToken cancellationToken)
        {
            try
            {
                await _wraService.SaveWRAAsync(wra, cancellationToken);
                return Ok(new
                {
                    success = true,
                    message = wra.Id == Guid.Empty
                       ? "WRA created successfully."
                       : "WRA updated successfully."
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
