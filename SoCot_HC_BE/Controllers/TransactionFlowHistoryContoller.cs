using Microsoft.AspNetCore.Mvc;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Helpers;
using SoCot_HC_BE.Services.Interfaces;
using System.Reflection;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TransactionFlowHistoryController : Controller
    {
        private readonly ITransactionFlowHistoryService _transactionFlowHistoryService;

        public TransactionFlowHistoryController(ITransactionFlowHistoryService transactionFlowHistoryService)
        {
            _transactionFlowHistoryService = transactionFlowHistoryService;
        }

        /// <summary>
        /// Updates the status of a transaction (must match with the module).
        /// </summary>
        [HttpPost("update-status")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusDto dto, CancellationToken cancellationToken)
        {
            if (dto == null || dto.TransactionId == Guid.Empty || dto.StatusId == null)
                return BadRequest("Invalid update request.");

            try
            {
                await _transactionFlowHistoryService.UpdateStatusAsync(dto, cancellationToken);

                return Ok(new { message = "Status updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
