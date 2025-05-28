using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCHC_API.Handler;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Personnels.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PersonnelController : Controller
    {
        private readonly IPersonnelService _personnelService;

        public PersonnelController(IPersonnelService personnelService)
        {
            _personnelService = personnelService;
        }

        // Get a specific personnel by ID
        [HttpGet("GetPersonnel/{id}")]
        public async Task<IActionResult> GetPersonnel(Guid id, CancellationToken cancellationToken)
        {
            var personnel = await _personnelService.GetAsync(id, cancellationToken);
            if (personnel == null)
            {
                return NotFound(new { success = false, message = "Personnel not found." });
            }

            return Ok(personnel);
        }

        // Get all personnel with paging
        [HttpGet("GetPagedPersonnel")]
        public async Task<IActionResult> GetPagedPersonnel(int pageNo, int limit, string? keyword, CancellationToken cancellationToken)
        {
            if (pageNo <= 0 || limit <= 0)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }

            var personnel = await _personnelService.GetAllWithPagingAsync(pageNo, limit, keyword, cancellationToken);
            var totalRecords = await _personnelService.CountAsync(keyword, cancellationToken);

            var paginatedResult = new PaginationHandler<Personnel>(personnel, totalRecords, pageNo, limit);
            return Ok(paginatedResult);
        }

        // Save or update a Personnel
        [HttpPost("SavePersonnel")]
        public async Task<IActionResult> SavePersonnel(Personnel personnel, CancellationToken cancellationToken)
        {
            try
            {
                await _personnelService.SavePersonnelAsync(personnel, cancellationToken);

                return Ok(new
                {
                    success = true,
                    message = personnel.PersonnelId == Guid.Empty
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