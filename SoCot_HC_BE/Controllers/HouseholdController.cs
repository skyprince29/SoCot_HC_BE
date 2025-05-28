using Microsoft.AspNetCore.Mvc;
using SCHC_API.Handler;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Model.Requests;
using SoCot_HC_BE.Services;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;
using SoCot_HC_BE.DTO;
using Microsoft.AspNetCore.Authorization;

namespace SoCot_HC_BE.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HouseholdController : ControllerBase
    {
        private readonly IHouseholdService _householdService;

        public HouseholdController(IHouseholdService householdService)
        {
            _householdService = householdService;
        }

        [HttpGet("GetPagedHousehold")]
        public async Task<IActionResult> GetPagedHousehold(int pageNo, int limit, string? keyword, CancellationToken cancellationToken)
        {
            if (pageNo <= 0 || limit <= 0)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }

            var households = await _householdService.GetAllWithPagingAsync(pageNo, limit, keyword, cancellationToken);
            var totalRecords = await _householdService.CountAsync(keyword, cancellationToken);

            var paginatedResult = new PaginationHandler<Household>(households, totalRecords, pageNo, limit);
            return Ok(paginatedResult);
        }


        [HttpGet("GetHousehold/{id}")]
        public async Task<IActionResult> GetService(Guid id, CancellationToken cancellationToken)
        {
            var service = await _householdService.GetAsync(id, cancellationToken);
            if (service == null)
            {
                return NotFound(new { success = false, message = "Service not found." });
            }

            return Ok(service);
        }

        [HttpPost("SaveHousehold")]
        public async Task<IActionResult> SaveHousehold([FromBody] SaveHouseholdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _householdService.SaveHouseholdAsync(request, cancellationToken);
                return Ok(new { success = true, message = "Household saved successfully." });
            }
            catch (ModelValidationException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Validation error",
                    errors = ex.Errors
                });
            }
            catch (OperationCanceledException)
            {
                return StatusCode(408, new
                {
                    success = false,
                    message = "Request Timeout. Please try again later."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An unexpected error occurred.",
                    details = ex.Message
                });
            }
        }

        [HttpPost("AppendFamilyToHousehold")]
        public async Task<IActionResult> AppendFamilyToHousehold([FromBody] AppendFamilyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _householdService.AppendFamilyToExistingHousehold(request, cancellationToken);
                return Ok(new { success = true, message = "Family added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error while appending family.",
                    details = ex.Message
                });
            }
        }

        [HttpPost("AddMemberToFamily")]
        public async Task<IActionResult> AddMemberToFamily([FromBody] AppendMemberRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _householdService.AppendMemberToExistingFamily(request, cancellationToken);
                return Ok(new { success = true, message = "Member added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error adding member.",
                    details = ex.Message
                });
            }
        }



    }
}
