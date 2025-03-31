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

            db.VitalSigns.Add(vitalSign);
            await db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetVitalSign), new { id = vitalSign.VitalSignId }, vitalSign);
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

            if (isValid)
            {
                ModelState.Clear();
            }
        }

        // api/v1/VitalSign/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVitalSign(int id)
        {
            var vitalsign = await db.VitalSigns.FindAsync(id);
            if (vitalsign == null)
                return NotFound();

            return Ok(vitalsign);
        }

        [HttpGet]
        public async Task<IActionResult> GetVitalSigns(int pageNo, int limit, string? keyword)
        {
            if (pageNo < 1 || limit < 1)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }

            int totalRecord = await db.VitalSigns.CountAsync();
            List<VitalSign> getVitalSign = await db.VitalSigns
                                            .Skip(pageNo)
                                            .Take(limit).ToListAsync();
            var paginatedResult = new PaginationHandler<VitalSign>(getVitalSign, totalRecord, pageNo, limit);
            return Ok(paginatedResult);
        }
    }
}
