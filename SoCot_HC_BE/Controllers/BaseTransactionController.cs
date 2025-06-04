using Microsoft.AspNetCore.Mvc;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Services.Interfaces;

namespace SoCot_HC_BE.Controllers
{
    public abstract class BaseTransactionController : Controller
    {
        protected readonly ITransactionFlowHistoryService _transactionFlowHistoryService;

        protected BaseTransactionController(ITransactionFlowHistoryService transactionFlowHistoryService)
        {
            _transactionFlowHistoryService = transactionFlowHistoryService;
        }

        // Must be defined in derived controllers
        protected abstract int ModuleId { get; }

        [HttpPost("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusDto dto, CancellationToken cancellationToken = default)
        {
            if (dto == null)
            {
                return BadRequest("Invalid update request.");
            }
            else
            {
                if(dto.TransactionId == Guid.Empty)
                    return BadRequest("Transaction id is required");
                if (!dto.StatusId.HasValue)
                    return BadRequest("Status is required");
                if (dto.Remarks == null || dto.Remarks == "")
                    return BadRequest("Remarks is required");
            }

            try
            {
                dto.ModuleId = ModuleId; // Enforce backend-only module ID
                await _transactionFlowHistoryService.UpdateStatusAsync(dto, cancellationToken);
                return Ok("Status updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
