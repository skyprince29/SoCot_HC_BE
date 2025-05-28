using Microsoft.AspNetCore.Mvc;
using SCHC_API.Handler;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Services;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class NonCommunicableDiseaseController : Controller
    {
        private readonly INonCommunicableDiseaseService _nonCommunicableDiseaseService;

        public NonCommunicableDiseaseController(INonCommunicableDiseaseService nonCommunicableDiseaseService)
        {
            _nonCommunicableDiseaseService = nonCommunicableDiseaseService;
        }


        [HttpGet("getNCDAsync")]
        public async Task<NonCommunicableDisease> getNCDAsync(Guid NCDId, CancellationToken cancellationToken)
        {
            return await _nonCommunicableDiseaseService.getNCDAsync(NCDId, cancellationToken);
        }


        [HttpGet("GetAllWithPagingAsync")]
        public async Task<IActionResult> GetAllWithPagingAsync(int pageNo, int limit, string keyword = "", CancellationToken cancellationToken = default) {

            if (pageNo <= 0 || limit <= 0)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }


            var paginatedResult = await _nonCommunicableDiseaseService.GetAllWithPagingAsync(pageNo, limit, keyword, cancellationToken);
            return Ok(paginatedResult);
        }

        [HttpPost("SaveOrUpdateDentalRecordAsync")]
        public async Task<IActionResult> SaveOrUpdateDentalRecordAsync(NonCommunicableDiseaseDto NDCDto, CancellationToken cancellationToken = default) {

            try
            {

                await _nonCommunicableDiseaseService.SaveOrUpdateDentalRecordAsync(NDCDto, cancellationToken);

                return Ok(new
                {
                    success = true,
                    message = NDCDto.Id == Guid.Empty
                        ? "Non Communicable Disease created successfully."
                        : "Non Communicable Disease updated successfully."
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

                return BadRequest(new
                {
                    success = false,
                    messge = "The request could not be processed due to invalid input. Please verify the submitted data and try again.",
                    errors = modelErrors
                }
                );
            }
        }

    }
}
