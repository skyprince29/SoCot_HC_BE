using Microsoft.AspNetCore.Mvc;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Services;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;
using System.Collections.Generic;
using System.Threading;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DentalRecordController : Controller
    {
        private readonly IDentalRecordService _dentalRecordService;

        public DentalRecordController(IDentalRecordService dentalRecordService)
        {
            _dentalRecordService = dentalRecordService;
        }


        [HttpGet("CreateDentalRecord")]
        public async Task<DentalRecord> CreateDentalRecord(String ReferralNo)
        {
            return await _dentalRecordService.CreateDentalRecord(ReferralNo);
        }

        [HttpGet("GetPagedDentalRecords")]
        public async Task<IActionResult> GetPagedDentalRecords(int pageNo, int limit, CancellationToken cancellationToken, string keyword = "")
        {
            if (pageNo <= 0 || limit <= 0)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }

            var paginatedResult = await _dentalRecordService.GetAllWithPagingAsync(pageNo, limit, keyword, cancellationToken);
            return Ok(paginatedResult);
        }

        [HttpPost("SaveOrUpdateDentalRecord")]
        public async Task<IActionResult> SaveOrUpdateDentalRecord(DentalRecord dentalRecord, CancellationToken cancellationToken)
        {
            try
            {

                await _dentalRecordService.SaveOrUpdateDentalRecord(dentalRecord, cancellationToken);

                return Ok(new
                {
                    success = true,
                    message = dentalRecord.DentalRecordId == Guid.Empty
                        ? "Department created successfully."
                        : "Department updated successfully."
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

        //[HttpGet("Create")]
        //public async Task<DentalRecord> Create(String ReferralNo)
        //{
        //    if (ReferralNo == "")
        //    {
        //        throw new Exception("Dental Treatment not found.");
        //        // BadRequest(new { message = "Page number and limit must be greater than zero." });
        //    }

        //    return await _dentalRecordService.CreateDentalRecord(ReferralNo);
        //}


    }
}
