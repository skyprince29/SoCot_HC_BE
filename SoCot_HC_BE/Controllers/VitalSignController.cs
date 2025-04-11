using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCHC_API.Handler;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Services.Interfaces;

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

        [HttpPost(Name = "SaveVitalSign")]
        public async Task<IActionResult> SaveVitalSign(VitalSign vitalSign)
        {
            bool isNew = vitalSign.VitalSignId == 0;

            if (vitalSign == null)
            {
                return BadRequest("VitalSign data is null.");
            }

            ValidateFields(vitalSign);

            if (!ModelState.IsValid)
            {
                var modelErrors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
                );
                return BadRequest(new { success = false, errors = modelErrors });
            }

            if (isNew)
            {
                await _vitalSignService.AddAsync(vitalSign);
                return CreatedAtAction(nameof(GetVitalSign), new { id = vitalSign.VitalSignId }, vitalSign);
            }
            else
            {
                var existingVitalSign = await _vitalSignService.GetAsync(vitalSign.VitalSignId);
                if (existingVitalSign == null)
                {
                    return NotFound(new { success = false, message = "VitalSign not found." });
                }

                await _vitalSignService.UpdateAsync(vitalSign);
                return Ok(new { success = true, message = "VitalSign updated successfully.", data = vitalSign });
            }
        }

        private void ValidateFields(VitalSign vitalSign)
        {
            // Validate Systolic
            if (vitalSign.Systolic == 0)
            {
                ModelState.AddModelError("Systolic", "Systolic is required");
            }
            else
            {
                // Clear previous validation error for this field if it's valid
                if (ModelState.ContainsKey("Systolic"))
                {
                    ModelState.Remove("Systolic");
                }
            }

            // Validate Diastolic
            if (vitalSign.Diastolic == 0)
            {
                ModelState.AddModelError("Diastolic", "Diastolic is required");
            }
            else
            {
                if (ModelState.ContainsKey("Diastolic"))
                {
                    ModelState.Remove("Diastolic");
                }
            }

            // Validate other fields similarly...
            if (vitalSign.Temperature == 0)
            {
                ModelState.AddModelError("Temperature", "Temperature is required");
            }
            else
            {
                if (ModelState.ContainsKey("Temperature"))
                {
                    ModelState.Remove("Temperature");
                }
            }

            if (vitalSign.Height == 0)
            {
                ModelState.AddModelError("Height", "Height is required");
            }
            else
            {
                if (ModelState.ContainsKey("Height"))
                {
                    ModelState.Remove("Height");
                }
            }

            if (vitalSign.Weight == 0)
            {
                ModelState.AddModelError("Weight", "Weight is required");
            }
            else
            {
                if (ModelState.ContainsKey("Weight"))
                {
                    ModelState.Remove("Weight");
                }
            }

            if (vitalSign.RespiratoryRate == 0)
            {
                ModelState.AddModelError("RespiratoryRate", "RespiratoryRate is required");
            }
            else
            {
                if (ModelState.ContainsKey("RespiratoryRate"))
                {
                    ModelState.Remove("RespiratoryRate");
                }
            }

            if (vitalSign.CardiacRate == 0)
            {
                ModelState.AddModelError("CardiacRate", "CardiacRate is required");
            }
            else
            {
                if (ModelState.ContainsKey("CardiacRate"))
                {
                    ModelState.Remove("CardiacRate");
                }
            }
            // If all fields are valid, you don't need to do anything further.
        }

        // api/v1/VitalSign/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVitalSign(long id)
        {
            var vitalSign = await _vitalSignService.GetAsync(id);
            if (vitalSign == null)
            {
                return NotFound(new { success = false, message = "VitalSign not found." });
            }

            return Ok(vitalSign);
        }

        [HttpGet]
        public async Task<IActionResult> GetVitalSigns(int pageNo, int limit, string? keyword)
        {
            if (pageNo <= 0 || limit <= 0)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }

            // Fetch paginated list of vital signs
            var vitalSigns = await _vitalSignService.GetAllWithPagingAsync(pageNo, limit, keyword);

            // Get the total count of records for pagination purposes
            var totalRecords = await _vitalSignService.CountAsync(keyword);

            var paginatedResult = new PaginationHandler<VitalSign>(vitalSigns, totalRecords, pageNo, limit);

            return Ok(paginatedResult);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVitalSign(long id)
        {
            var existingVitalSign = await _vitalSignService.GetAsync(id);
            if (existingVitalSign == null)
            {
                return NotFound(new { success = false, message = "VitalSign not found." });
            }

            await _vitalSignService.DeleteAsync(id);
            return Ok(new { success = true, message = "VitalSign deleted successfully." });
        }
    }
}
