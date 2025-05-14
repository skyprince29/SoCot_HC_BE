using Microsoft.AspNetCore.Mvc;
using SCHC_API.Handler;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SupplyStorageController : Controller
    {
        private readonly ISupplyStorageService _supplyStorageService;

        public SupplyStorageController(ISupplyStorageService supplyStorageService)
        {
            _supplyStorageService = supplyStorageService;
        }

        // Get all Supply Storages with paging
        [HttpGet("GetPagedSupplyStorages")]
        public async Task<IActionResult> GetPagedSupplyStorages(int pageNo, int limit, string? keyword, CancellationToken cancellationToken)
        {
            if (pageNo <= 0 || limit <= 0)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }

            var supplyStorages = await _supplyStorageService.GetAllWithPagingAsync(pageNo, limit, keyword, cancellationToken);
            var totalRecords = await _supplyStorageService.CountAsync(keyword, cancellationToken);

            var paginatedResult = new PaginationHandler<SupplyStorage>(supplyStorages, totalRecords, pageNo, limit);
            return Ok(paginatedResult);
        }

        // Get a specific supply storage by ID
        [HttpGet("GetSupplyStorage/{id}")]
        public async Task<IActionResult> GetSupplyStorage(Guid id, CancellationToken cancellationToken)
        {
            var service = await _supplyStorageService.GetAsync(id, cancellationToken);
            if (service == null)
            {
                return NotFound(new { success = false, message = "Supply Storage not found." });
            }

            return Ok(service);
        }

        // Save or update a SupplyStorage
        [HttpPost("SaveSupplyStorage")]
        public async Task<IActionResult> SaveSupplyStorage(SupplyStorageDto supplyStorage, CancellationToken cancellationToken)
        {
            try
            {
                await _supplyStorageService.SaveSupplyStorageAsync(supplyStorage, cancellationToken);

                return Ok(new
                {
                    success = true,
                    message = supplyStorage.SupplyStorageId == Guid.Empty
                        ? "Supply Storage created successfully."
                        : "Supply Storage updated successfully."
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
