using Microsoft.AspNetCore.Mvc;
using SCHC_API.Handler;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class VitalSignController : Controller
    {
        private readonly IVitalSignService _vitalSignService;

        public VitalSignController(IVitalSignService vitalSignService)
        {
            _vitalSignService = vitalSignService;
        }

        [HttpPost("SaveVitalSign")]
        public async Task<IActionResult> SaveVitalSign(VitalSignDto vitalSign, CancellationToken cancellationToken)
        {
            try
            {
                await _vitalSignService.SaveVitalSignAsync(vitalSign, cancellationToken);

                return Ok(new
                {
                    success = true,
                    message = vitalSign.VitalSignId == Guid.Empty
                        ? "Vital Sign created successfully."
                        : "Vital Sign updated successfully."
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

        // Get a specific VitalSign by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVitalSign(Guid id, CancellationToken cancellationToken)
        {
            var vitalSign = await _vitalSignService.GetAsync(id, cancellationToken);
            if (vitalSign == null)
            {
                return NotFound(new { success = false, message = "VitalSign not found." });
            }

            return Ok(vitalSign);
        }

        // Get all VitalSigns with paging
        [HttpGet]
        public async Task<IActionResult> GetVitalSigns(int pageNo, int limit, string? keyword, CancellationToken cancellationToken)
        {
            if (pageNo <= 0 || limit <= 0)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }

            var vitalSigns = await _vitalSignService.GetAllWithPagingAsync(pageNo, limit, keyword, cancellationToken);
            var totalRecords = await _vitalSignService.CountAsync(keyword, cancellationToken);

            var paginatedResult = new PaginationHandler<VitalSign>(vitalSigns, totalRecords, pageNo, limit);
            return Ok(paginatedResult);
        }

        // Delete a VitalSign
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVitalSign(Guid id, CancellationToken cancellationToken)
        {
            var existingVitalSign = await _vitalSignService.GetAsync(id, cancellationToken);
            if (existingVitalSign == null)
            {
                return NotFound(new { success = false, message = "VitalSign not found." });
            }

            await _vitalSignService.DeleteAsync(id, cancellationToken);
            return Ok(new { success = true, message = "VitalSign deleted successfully." });
        }
    }
}
