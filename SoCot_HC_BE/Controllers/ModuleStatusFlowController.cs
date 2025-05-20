using Microsoft.AspNetCore.Mvc;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Services.Interfaces;

namespace SoCot_HC_BE.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ModuleStatusFlowController : Controller
    {
        private readonly IModuleStatusFlowService _moduleStatusFlowService;

        public ModuleStatusFlowController(IModuleStatusFlowService moduleStatusFlowService)
        {
            _moduleStatusFlowService = moduleStatusFlowService;
        }

        [HttpGet("GetAllStatusFlows/{moduleId}")]
        public async Task<ActionResult<List<ModuleStatusFlow>>> GetAllStatusFlows(int moduleId, CancellationToken cancellationToken)
        {
            if (moduleId <= 0)
                return BadRequest("Invalid moduleId.");

            var flows = await _moduleStatusFlowService.GetAllStatusFlowsAsync(moduleId, cancellationToken);
            return Ok(flows);
        }

        [HttpGet("GetAllStatusesByModule/{moduleId}")]
        public async Task<ActionResult<List<Status>>> GetAllStatusesByModule(int moduleId, CancellationToken cancellationToken)
        {
            if (moduleId <= 0)
                return BadRequest("Invalid moduleId.");

            var statuses = await _moduleStatusFlowService.GetAllStatusesByModuleAsync(moduleId, cancellationToken);
            return Ok(statuses);
        }

        [HttpGet("GetFirstStatus/{moduleId}")]
        public async Task<ActionResult<Status?>> GetFirstStatus(int moduleId, CancellationToken cancellationToken)
        {
            if (moduleId <= 0)
                return BadRequest("Invalid moduleId.");

            var status = await _moduleStatusFlowService.GetFirstStatusAsync(moduleId, cancellationToken);
            if (status == null)
                return NotFound("First status not found.");

            return Ok(status);
        }

        [HttpGet("IsCompleteStatus")]
        public async Task<ActionResult<bool>> IsCompleteStatus(int moduleId, byte statusId, CancellationToken cancellationToken)
        {
            if (moduleId <= 0 || statusId <= 0)
                return BadRequest("Invalid moduleId or statusId.");

            var result = await _moduleStatusFlowService.IsCompleteStatusAsync(moduleId, statusId, cancellationToken);
            return Ok(result);
        }

        [HttpGet("IsStartStatus")]
        public async Task<ActionResult<bool>> IsStartStatus(int moduleId, byte statusId, CancellationToken cancellationToken)
        {
            if (moduleId <= 0 || statusId <= 0)
                return BadRequest("Invalid moduleId or statusId.");

            var result = await _moduleStatusFlowService.IsStartStatusAsync(moduleId, statusId, cancellationToken);
            return Ok(result);
        }

        [HttpGet("GetNextStatuses")]
        public async Task<ActionResult<List<Status>>> GetNextStatuses(int moduleId, byte currentStatusId, CancellationToken cancellationToken)
        {
            if (moduleId <= 0 || currentStatusId <= 0)
                return BadRequest("Invalid moduleId or currentStatusId.");

            var result = await _moduleStatusFlowService.GetNextStatusesAsync(moduleId, currentStatusId, cancellationToken);
            return Ok(result);
        }
    }
}
