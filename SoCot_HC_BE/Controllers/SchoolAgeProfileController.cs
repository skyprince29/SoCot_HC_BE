using Microsoft.AspNetCore.Mvc;
using SCHC_API.Handler;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Utils;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Services;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SchoolAgeProfileController : Controller
    {
        private readonly ISchoolAgeProfileService _schoolageprofileService;

        public SchoolAgeProfileController(ISchoolAgeProfileService schoolageprofileService)
        {
            _schoolageprofileService = schoolageprofileService;
        }

        // Get a specific SchoolAgeProfile by ID
        [HttpGet("GetSchoolAgeProfile/{id}")]
        public async Task<IActionResult> GetSchoolAgeProfile(Guid id, CancellationToken cancellationToken)
        {
            var schoolageprofile = await _schoolageprofileService.GetAsync(id, cancellationToken);
            if (schoolageprofile == null)
            {
                return NotFound(new { success = false, message = "School Age Profile not found." });
            }

            return Ok(schoolageprofile);
        }

        // Get all SchoolAgeProfiles with paging
        [HttpGet("GetPagedSchoolAgeProfiles")]
        public async Task<IActionResult> GetPagedSchoolAgeProfiles(int pageNo, int limit, string? keyword, CancellationToken cancellationToken)
        {
            if (pageNo <= 0 || limit <= 0)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }

            var schoolageprofile = await _schoolageprofileService.GetAllWithPagingAsync(pageNo, limit, keyword, cancellationToken);
            var totalRecords = await _schoolageprofileService.CountAsync(keyword, cancellationToken);

            var paginatedResult = new PaginationHandler<SchoolAgeProfile>(schoolageprofile, totalRecords, pageNo, limit);
            return Ok(paginatedResult);
        }

        [HttpPost("SaveSchoolAgeProfile")]
        public async Task<IActionResult> SaveSchoolAgeProfile(SchoolAgeProfileDto schoolageprofile, CancellationToken cancellationToken)
         {
            try
            {
                await _schoolageprofileService.SaveSchoolAgeProfileAsync(schoolageprofile, cancellationToken);
                return Ok(new
                {
                    success = true,
                    message = schoolageprofile.SchoolAgeProfileId == Guid.Empty
                       ? "SchoolAgeProfile created successfully."
                       : "SchoolAgeProfile updated successfully."
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
