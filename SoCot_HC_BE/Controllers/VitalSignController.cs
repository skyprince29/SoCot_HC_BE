using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCHC_API.Handler;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.Model;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class VitalSignController : Controller
    {
        private readonly AppDbContext db;
        public VitalSignController(AppDbContext context)
        {
            db = context;
        }

        [HttpPost(Name = "SaveVitalSign")]
        public async Task<IActionResult> SaveVitalSign(VitalSign vitalSign)
        {
            bool isNew = vitalSign.VitalSignId == 0;
            if (vitalSign == null)
            {
                return BadRequest("VitalSign data is null.");
            }
            //ModelState.Clear();
            ValidateFields(vitalSign);
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
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
                db.VitalSigns.Add(vitalSign);
                await db.SaveChangesAsync();
                return CreatedAtAction(nameof(GetVitalSign), new { id = vitalSign.VitalSignId }, vitalSign);
            }
            else
            {
                var existingVitalSign = await db.VitalSigns.FindAsync(vitalSign.VitalSignId);
                if (existingVitalSign == null)
                {
                    return NotFound(new { success = false, message = "VitalSign not found." });
                }
                db.Entry(existingVitalSign).CurrentValues.SetValues(vitalSign);
                await db.SaveChangesAsync();
                return Ok(new { success = true, message = "VitalSign updated successfully.", data = vitalSign });
            }
        }

        private void ValidateFields(VitalSign vitalSign)
        {

            bool isValid = true;
            if (vitalSign.Systolic == 0)
            {
                ModelState.AddModelError("Systolic", "Systolic is required");
                isValid = false;
            }
            else
            {
                ModelState.AddModelError("Systolic", "");
            }

            if (vitalSign.Diastolic == 0)
            {
                ModelState.AddModelError("Diastolic", "Systolic is required");
                isValid = false;
            } else
            {
                ModelState.AddModelError("Diastolic", "");
            }

            if (vitalSign.Diastolic == 0)
            {
                ModelState.AddModelError("Temperature", "Temperature is required");
                isValid = false;
            }
            else
            {
                ModelState.AddModelError("Temperature", "");
            }

            if (vitalSign.Diastolic == 0)
            {
                ModelState.AddModelError("Height", "Height is required");
                isValid = false;
            }
            else
            {
                ModelState.AddModelError("Height", "");
            }


            if (vitalSign.Diastolic == 0)
            {
                ModelState.AddModelError("Weight", "Weight is required");
                isValid = false;
            }
            else
            {
                ModelState.AddModelError("Weight", "");
            }

            if (vitalSign.Diastolic == 0)
            {
                ModelState.AddModelError("RespiratoryRate", "RespiratoryRate is required");
                isValid = false;
            }
            else
            {
                ModelState.AddModelError("RespiratoryRate", "");
            }

            if (vitalSign.Diastolic == 0)
            {
                ModelState.AddModelError("CardiacRate", "CardiacRate is required");
                isValid = false;
            }
            else
            {
                ModelState.AddModelError("CardiacRate", "");
            }
            if (isValid)
            {
                ModelState.Clear();
            }
        }

        // api/v1/VitalSign/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVitalSign(long id)
        {
            var vitalsign = await db.VitalSigns.FindAsync(id);
            if (vitalsign == null)
            {
                return NotFound(new { success = false, message = "VitalSign not found." });
            }

            return Ok(vitalsign);
        }

        [HttpGet]
        public async Task<IActionResult> GetVitalSigns(int pageNo, int limit, string? keyword)
        {
            if (pageNo < 0 || limit < 0)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }
            int skip = (pageNo - 1);
            int totalRecord = await db.VitalSigns.CountAsync();
            List<VitalSign> getVitalSign = await db.VitalSigns
                                            .Skip(skip)
                                            .Take(limit).ToListAsync();
            var paginatedResult = new PaginationHandler<VitalSign>(getVitalSign, totalRecord, pageNo, limit);
            return Ok(paginatedResult);
        }


    }
}
