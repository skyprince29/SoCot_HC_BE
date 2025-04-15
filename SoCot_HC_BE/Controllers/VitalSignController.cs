using Microsoft.AspNetCore.Mvc;
using SCHC_API.Handler;
using SoCot_HC_BE.Model;

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

        // Save or update a VitalSign
        [HttpPost(Name = "SaveVitalSign")]
        public async Task<IActionResult> SaveVitalSign(VitalSign vitalSign, CancellationToken cancellationToken)
        {
            bool isNew = vitalSign.VitalSignId == Guid.Empty;

            if (vitalSign == null)
            {
                return BadRequest("VitalSign data is null.");
            }

            // Validate fields before processing
            ValidateFields(vitalSign);

            // If the model is not valid, return errors
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToList()
                );
                return BadRequest(new { success = false, errors = modelErrors });
            }

            if (isNew)
            {
                await _vitalSignService.AddAsync(vitalSign, cancellationToken);
                return CreatedAtAction(nameof(GetVitalSign), new { id = vitalSign.VitalSignId }, vitalSign);
            }
            else
            {
                var existingVitalSign = await _vitalSignService.GetAsync(vitalSign.VitalSignId, cancellationToken);
                if (existingVitalSign == null)
                {
                    return NotFound(new { success = false, message = "VitalSign not found." });
                }

                await _vitalSignService.UpdateAsync(vitalSign, cancellationToken);
                return Ok(new { success = true, message = "VitalSign updated successfully.", data = vitalSign });
            }
        }

        // Method to validate fields in the VitalSign model
        private void ValidateFields(VitalSign vitalSign)
        {
            // Validate Systolic
            if (vitalSign.Systolic == 0)
            {
                ModelState.AddModelError("Systolic", "Systolic is required.");
            }
            else
            {
                if (ModelState.ContainsKey("Systolic"))
                {
                    ModelState.Remove("Systolic");
                }
            }

            // Validate Diastolic
            if (vitalSign.Diastolic == 0)
            {
                ModelState.AddModelError("Diastolic", "Diastolic is required.");
            }
            else
            {
                if (ModelState.ContainsKey("Diastolic"))
                {
                    ModelState.Remove("Diastolic");
                }
            }

            // Validate Temperature
            if (vitalSign.Temperature == 0)
            {
                ModelState.AddModelError("Temperature", "Temperature is required.");
            }
            else
            {
                if (ModelState.ContainsKey("Temperature"))
                {
                    ModelState.Remove("Temperature");
                }
            }

            // Validate Height
            if (vitalSign.Height == 0)
            {
                ModelState.AddModelError("Height", "Height is required.");
            }
            else
            {
                if (ModelState.ContainsKey("Height"))
                {
                    ModelState.Remove("Height");
                }
            }

            // Validate Weight
            if (vitalSign.Weight == 0)
            {
                ModelState.AddModelError("Weight", "Weight is required.");
            }
            else
            {
                if (ModelState.ContainsKey("Weight"))
                {
                    ModelState.Remove("Weight");
                }
            }

            // Validate Respiratory Rate
            if (vitalSign.RespiratoryRate == 0)
            {
                ModelState.AddModelError("RespiratoryRate", "Respiratory Rate is required.");
            }
            else
            {
                if (ModelState.ContainsKey("RespiratoryRate"))
                {
                    ModelState.Remove("RespiratoryRate");
                }
            }

            // Validate Cardiac Rate
            if (vitalSign.CardiacRate == 0)
            {
                ModelState.AddModelError("CardiacRate", "Cardiac Rate is required.");
            }
            else
            {
                if (ModelState.ContainsKey("CardiacRate"))
                {
                    ModelState.Remove("CardiacRate");
                }
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
