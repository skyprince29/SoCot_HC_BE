using Microsoft.AspNetCore.Mvc;
using SCHC_API.Handler;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Designations;
using SoCot_HC_BE.Designations.Interfaces;
using SoCot_HC_BE.Utils;
using Microsoft.AspNetCore.Authorization;

namespace SoCot_HC_BE.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DesignationController : Controller
    {
        private readonly IDesignationService _designationService;

        public DesignationController(IDesignationService designationService)
        {
            _designationService = designationService;
        }

        // Get a specific Designation by ID
        [HttpGet("GetDesignation/{id}")]
        public async Task<IActionResult> GetDesignation(Guid id, CancellationToken cancellationToken)
        {
            var designation = await _designationService.GetAsync(id, cancellationToken);
            if (designation == null)
            {
                return NotFound(new { success = false, message = "Designation not found." });
            }

            return Ok(designation);
        }

        [HttpGet("GetDesignations")]
        public async Task<IActionResult> GetDesignations(
        [FromQuery] bool isActiveOnly = true,
        [FromQuery] Guid? currentId = null, // ✅ correct
        CancellationToken cancellationToken = default)
        {
            IEnumerable<Designation> items;

            if (isActiveOnly && currentId.HasValue && currentId.Value != Guid.Empty)
            {
                items = await _designationService.GetAllActiveWithCurrentAsync(currentId.Value, cancellationToken);
            }
            else if (isActiveOnly)
            {
                items = await _designationService.GetAllActiveOnlyAsync(cancellationToken);
            }
            else
            {
                items = await _designationService.GetAllAsync(cancellationToken);
            }

            return Ok(items);
        }

        // Get all Designation with paging
        [HttpGet("GetPagedDesignations")]
        public async Task<IActionResult> GetPagedDesignations(int pageNo, int limit, string? keyword, CancellationToken cancellationToken)
        {
            if (pageNo <= 0 || limit <= 0)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }

            var designations = await _designationService.GetAllWithPagingAsync(pageNo, limit, keyword, cancellationToken);
            var totalRecords = await _designationService.CountAsync(keyword, cancellationToken);

            var paginatedResult = new PaginationHandler<Designation>(designations, totalRecords, pageNo, limit);
            return Ok(paginatedResult);
        }

        [HttpPost("SaveDesignation")]
        public async Task<IActionResult> SaveDesignation(Designation designation, CancellationToken cancellationToken)
        {
            try
            {
                await _designationService.SaveDesignationAsync(designation, cancellationToken);
                return Ok(new
                {
                    success = true,
                    message = designation.DesignationId == Guid.Empty
                       ? "Personnel created successfully."
                       : "Personnel updated successfully."
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
