using Microsoft.AspNetCore.Mvc;
using SCHC_API.Handler;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.DTO.ParamDto;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Model.Enums;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FacilityReferralController : BaseTransactionController
    {
        private readonly IFacilityReferralService _facilityReferralService;
        protected override int ModuleId => (int)ModuleEnum.Referral;

        public FacilityReferralController(ITransactionFlowHistoryService _transactionFlowHistoryService, IFacilityReferralService facilityReferralService) : base(_transactionFlowHistoryService)
        {
            _facilityReferralService = facilityReferralService;
        }

        // Save or update a referral
        [HttpPost("SaveReferral")]
        public async Task<IActionResult> SaveReferral(FacilityReferralDto facilityReferralDto, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Call the updated service method which now returns the saved entity.
                //    Assuming 'isWithValidation' should be true when called from the controller.
                var savedRegistry = await _facilityReferralService.SaveReferralAsync(facilityReferralDto, cancellationToken);

                // 2. Return a success response including the ID from the object we got back.
                return Ok(new
                {
                    success = true,
                    message = facilityReferralDto.ReferralId == Guid.Empty
                        ? "Referral created successfully."
                        : "Referral updated successfully.",
                    Id = savedRegistry.ReferralId // <-- The returned ID
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

        [HttpGet("GetReferral/{id}")]
        public async Task<IActionResult> GetReferral(Guid id, CancellationToken cancellationToken)
        {
            var patientRegistry = await _facilityReferralService.GetAsync(id, cancellationToken);
            if (patientRegistry == null)
            {
                return NotFound(new { success = false, message = "Referral not found." });
            }

            return Ok(patientRegistry);
        }

        [HttpGet("GetPagedReferrals")]
        public async Task<IActionResult> GetPagedReferrals(
           [FromQuery] GetPagedReferralParam request,
           CancellationToken cancellationToken)
        {
            if (request.PageNo <= 0 || request.Limit <= 0)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }

            var referrals = await _facilityReferralService.GetAllWithPagingAsync(
                request, cancellationToken);

            var totalRecords = await _facilityReferralService.CountAsync(
                request, cancellationToken);

            var paginatedResult = new PaginationHandler<Referral>(referrals, totalRecords, request.PageNo, request.Limit);
            return Ok(paginatedResult);
        }
    }
}
