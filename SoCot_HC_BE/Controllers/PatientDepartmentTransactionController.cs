using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SCHC_API.Handler;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.DTO.ParamDto;
using SoCot_HC_BE.Hub;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Model.Enums;
using SoCot_HC_BE.Services.Interfaces;
using SoCot_HC_BE.Utils;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PatientDepartmentTransactionController : BaseTransactionController
    {
        private readonly IHubContext<AppHub> _appHubContext; // <-- Changed type

        private readonly IPatientDepartmentTransactionService _patientDepartmentTransactionService;
        protected override int ModuleId => (int)ModuleEnum.PatientDepartmentTransaction;

        public PatientDepartmentTransactionController(ITransactionFlowHistoryService _transactionFlowHistoryService,
            IPatientDepartmentTransactionService patientDepartmentTransactionService,
            IHubContext<AppHub> appHubContext) : base(_transactionFlowHistoryService)
        {
            _patientDepartmentTransactionService = patientDepartmentTransactionService;
            _appHubContext = appHubContext;
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
            [FromQuery] GetPagedPDTRequestParam request,
            CancellationToken cancellationToken)
        {
            if (request.PageNo <= 0 || request.Limit <= 0)
            {
                return BadRequest(new { message = "Page number and limit must be greater than zero." });
            }

            var patientDT = await _patientDepartmentTransactionService.GetAllWithPagingAsync(
                request, cancellationToken);

            var totalRecords = await _patientDepartmentTransactionService.CountAsync(
                request, cancellationToken);

            var paginatedResult = new PaginationHandler<PatientDepartmentTransaction>(patientDT, totalRecords, request.PageNo, request.Limit);
            return Ok(paginatedResult);
        }


        [HttpPatch("DefferTransaction")]
        public async Task<IActionResult> DefferTransaction([FromBody] UpdateStatusDto dto, CancellationToken cancellationToken)
        {
            var success = await _patientDepartmentTransactionService.UpdateDefferedByAsync(dto, cancellationToken);

            if (!success)
                return NotFound("Transaction not found.");

            await _appHubContext.Clients.All.SendAsync("ReloadPageAsyncSignalR", "Deffered Transaction. Reload page");
            return Ok("Transaction deffered successfully.");
        }


        [HttpPatch("AcceptTransaction")]
        public async Task<IActionResult> AcceptTransaction([FromBody] AcceptTransactionDto dto)
        {
            var success = await _patientDepartmentTransactionService.UpdateAcceptedByAsync(dto.Id, dto.AcceptedBy);

            if (!success)
                return NotFound("Transaction not found.");

            await _appHubContext.Clients.All.SendAsync("ReloadPageAsyncSignalR", "Accepted Transaction. Reload page");
            return Ok("Transaction accepted successfully.");
        }

        [HttpPost("ForwardPatient")]
        public async Task<IActionResult> ForwardPatient(PatientForwardingDto dto, CancellationToken cancellationToken)
        {
            try
            {
                PatientDepartmentTransaction savedPDT = await _patientDepartmentTransactionService.CreateTransactionAsync(dto, cancellationToken);
                
                await _appHubContext.Clients.All.SendAsync("ReloadPageAsyncSignalR", "Forwarded Transaction. Reload page");
                return Ok(new
                {
                    success = true,
                    message = "Patient forwarded successfully.",
                    Id = savedPDT.PatientRegistryId // <-- The returned ID
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



        [HttpGet("GetPatientDeptmentTransactionVitalsignAsync/{id}")]
        public async Task<IActionResult> GetPatientDeptmentTransactionVitalsignAsync(Guid id, CancellationToken cancellationToken)
        {
            var transaction = await _patientDepartmentTransactionService.GetPatientDeptmentTransactionVitalsignAsync(id, cancellationToken);

            if (transaction == null)
            {
                return NotFound(new { success = false, message = "Transaction not found." });
            }

            return Ok(transaction);
        }

    }
}
