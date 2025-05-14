using Microsoft.AspNetCore.Mvc;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Services.Interfaces;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DentalTreatmentController : Controller
    {
        private readonly IDentalTreatmentService _dentalTreatmentService;

        public DentalTreatmentController(IDentalTreatmentService dentalTreatmentService)
        {
            _dentalTreatmentService = dentalTreatmentService;
        }

        [HttpGet("GetPagedDentalTreatments")]
        public async Task<IActionResult> GetPagedDentalTreatments(int pageNo, int limit, CancellationToken cancellationToken, string keyword = "")
        {
            if (pageNo <= 0 || limit <= 0)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }

            var paginatedResult = await _dentalTreatmentService.GetAllWithPagingAsync(pageNo, limit, keyword, cancellationToken);
            return Ok(paginatedResult);
        }

    }
}
