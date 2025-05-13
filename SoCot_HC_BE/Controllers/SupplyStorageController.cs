using Microsoft.AspNetCore.Mvc;
using SCHC_API.Handler;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Services.Interfaces;

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
    }
}
