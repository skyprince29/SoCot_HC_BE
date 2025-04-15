using Microsoft.AspNetCore.Mvc;
using SCHC_API.Handler;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FacilityController : Controller
    {
        private readonly IFacilityService _facilityService;

        public FacilityController(IFacilityService facilityService)
        {
            _facilityService = facilityService;
        }

        // Get a specific Facility by ID
        [HttpGet("GetFacility/{id}")]
        public async Task<IActionResult> GetFacility(int id, CancellationToken cancellationToken)
        {
            var facility = await _facilityService.GetAsync(id, cancellationToken);
            if (facility == null)
            {
                return NotFound(new { success = false, message = "Facility not found." });
            }

            return Ok(facility);
        }

        // Get all Facility with paging
        [HttpGet("GetPagedFacilities")]
        public async Task<IActionResult> GetPagedFacilities(int pageNo, int limit, string? keyword, CancellationToken cancellationToken)
        {
            if (pageNo <= 0 || limit <= 0)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }

            var facilities = await _facilityService.GetAllWithPagingAsync(pageNo, limit, keyword, cancellationToken);
            var totalRecords = await _facilityService.CountAsync(keyword, cancellationToken);

            var paginatedResult = new PaginationHandler<Facility>(facilities, totalRecords, pageNo, limit);
            return Ok(paginatedResult);
        }

        // Get facilities
        [HttpGet("GetFacilities")]
        public async Task<IActionResult> GetFacilities(CancellationToken cancellationToken)
        {
            var items = await _facilityService.GetAllAsync(cancellationToken);
            return Ok(items);
        }

        // Get all active facilities
        [HttpGet("GetAllActiveFacilities")]
        public async Task<IActionResult> GetAllActiveFacilities(CancellationToken cancellationToken)
        {
            var items = await _facilityService.GetAllActiveOnlyAsync(cancellationToken);
            return Ok(items);
        }

        // Get active facilties including a specific item
        [HttpGet("GetAllActiveFacilities/{currentId}")]
        public async Task<IActionResult> GetAllActiveWithCurrentFaciltities(int currentId, CancellationToken cancellationToken)
        {
            var items = await _facilityService.GetAllActiveWithCurrentAsync(currentId, cancellationToken);
            return Ok(items);
        }

        [HttpPost("SaveFacility")]
        public async Task<IActionResult> SaveFacility(Facility facility, CancellationToken cancellationToken)
        {
            try
            {
                await _facilityService.SaveFacilityAsync(facility, cancellationToken);

                return Ok(new
                {
                    success = true,
                    message = facility.FacilityId == 0
                        ? "Facility created successfully."
                        : "Facility updated successfully."
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
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
                );

                return BadRequest(new { success = false, errors = modelErrors });
            }
        }
    }
}
