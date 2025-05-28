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
    public class PatientDepartmentTransactionController : Controller
    {
        private readonly IPatientDepartmentTransactionService _patientDepartmentTransactionService;

        public PatientDepartmentTransactionController(IPatientDepartmentTransactionService patientDepartmentTransactionService)
        {
            _patientDepartmentTransactionService = patientDepartmentTransactionService;
        }

        [HttpGet("GetTransaction/{id}")]
        public async Task<IActionResult> GetTransaction(Guid id, CancellationToken cancellationToken)
        {
            var transaction = await _patientDepartmentTransactionService.GetAsync(id, cancellationToken);

            if (transaction == null)
            {
                return NotFound(new { success = false, message = "Transaction not found." });
            }

            return Ok(transaction);
        }

        [HttpGet("GetPagedPatientDepartmentTransactions")]
        public async Task<IActionResult> GetPagedPatientDepartmentTransactions(
            [FromQuery] Guid fromDepartmentId,
            [FromQuery] Guid currentDepartmentId,
            [FromQuery] int pageNo,
            [FromQuery] int limit,
            [FromQuery] string? keyword,
            [FromQuery] byte? status,
            CancellationToken cancellationToken)
        {
            if (pageNo <= 0 || limit <= 0)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }

            var patientDT = await _patientDepartmentTransactionService.GetAllWithPagingAsync(
                fromDepartmentId, currentDepartmentId, pageNo, limit, keyword, status, cancellationToken);

            var totalRecords = await _patientDepartmentTransactionService.CountAsync(
                fromDepartmentId, currentDepartmentId, keyword, status, cancellationToken);

            var paginatedResult = new PaginationHandler<PatientDepartmentTransaction>(patientDT, totalRecords, pageNo, limit);
            return Ok(paginatedResult);
        }

        [HttpPatch("AcceptTransaction")]
        public async Task<IActionResult> AcceptTransaction([FromBody] AcceptTransactionDto dto)
        {
            var success = await _patientDepartmentTransactionService.UpdateAcceptedByAsync(dto.Id, dto.AcceptedBy);

            if (!success)
                return NotFound("Transaction not found.");

            return Ok("Transaction accepted successfully.");
        }

        [HttpPost("ForwardPatient")]
        public async Task<IActionResult> ForwardPatient(PatientForwardingDto dto, CancellationToken cancellationToken)
        {
            try
            {
                await _patientDepartmentTransactionService.CreateTransactionAsync(dto, cancellationToken);

                return Ok(new
                {
                    success = true,
                    message = "Patient forwarded successfully."
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
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An unexpected error occurred.",
                    detail = ex.Message
                });
            }
        }

    }
}
