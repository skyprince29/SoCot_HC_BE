using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Data;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;

namespace SoCot_HC_BE.Services.Interfaces
{
    public class ModuleStatusFlowService : Repository<ModuleStatusFlow, int>, IModuleStatusFlowService
    {
        public ModuleStatusFlowService(AppDbContext context) : base(context)
        {
        }

        public async Task<List<ModuleStatusFlow>> GetAllStatusFlowsAsync(int moduleId, CancellationToken cancellationToken = default)
        {
            var flows = await _dbSet
                .Include(m => m.RequiredStatus)
                .Include(m => m.NextStatus)
                .Where(m => m.ModuleId == moduleId)
                .ToListAsync(cancellationToken);

            var orderedFlows = new List<ModuleStatusFlow>();

            // Step 1: Get the starting flow (RequiredStatusId == null)
            var startFlow = flows.FirstOrDefault(f => f.RequiredStatusId == null);
            if (startFlow != null)
            {
                orderedFlows.Add(startFlow);

                // Step 2: Follow the flow chain using RequiredStatusId and NextStatusId
                int? currentStatusId = startFlow.NextStatusId;
                while (currentStatusId != null)
                {
                    var nextFlow = flows.FirstOrDefault(f => f.RequiredStatusId == currentStatusId && f.NextStatusId != 3); // exclude Cancelled
                    if (nextFlow == null)
                        break;

                    if (!orderedFlows.Any(f => f.Id == nextFlow.Id))
                        orderedFlows.Add(nextFlow);

                    currentStatusId = nextFlow.NextStatusId;
                }

                // Step 3: Add Cancelled flow (NextStatusId == 3), if any
                var cancelledFlow = flows.FirstOrDefault(f => f.NextStatusId == 3);
                if (cancelledFlow != null && !orderedFlows.Any(f => f.Id == cancelledFlow.Id))
                {
                    orderedFlows.Add(cancelledFlow);
                }
            }

            return orderedFlows;
        }

        public async Task<List<Status>> GetAllStatusesByModuleAsync(int moduleId, CancellationToken cancellationToken = default)
        {
            // Get the full status flow for the given module
            var flows = await (from wf in _dbSet
                               where wf.ModuleId == moduleId
                               join status in _context.Status on wf.NextStatusId equals status.Id
                               select new { wf.RequiredStatusId, Status = status })
                              .ToListAsync(cancellationToken);

            // Track unique statuses
            var statusList = new List<Status>();

            // 1. Add the starting status (RequiredStatusId == null)
            var startStatus = flows.FirstOrDefault(f => f.RequiredStatusId == null)?.Status;
            if (startStatus != null && !statusList.Any(s => s.Id == startStatus.Id))
            {
                statusList.Add(startStatus);
            }

            // 2. Add other statuses in the order they appear, excluding Cancelled (Id = 3)
            foreach (var flow in flows)
            {
                if (flow.Status.Id != 3 && !statusList.Any(s => s.Id == flow.Status.Id))
                {
                    statusList.Add(flow.Status);
                }
            }

            // 3. Add Cancelled status (Id = 3) at the end if it exists
            var cancelledStatus = flows.FirstOrDefault(f => f.Status.Id == 3)?.Status;
            if (cancelledStatus != null && !statusList.Any(s => s.Id == 3))
            {
                statusList.Add(cancelledStatus);
            }

            return statusList;
        }

        public async Task<Status?> GetFirstStatusAsync(int moduleId, CancellationToken cancellationToken = default)
        {
            var firstFlow = await _dbSet
                .Include(f => f.NextStatus)
                .FirstOrDefaultAsync(f => f.ModuleId == moduleId && f.RequiredStatusId == null, cancellationToken);

            return firstFlow?.NextStatus;
        }

        public async Task<bool> IsCompleteStatusAsync(int moduleId, byte statusId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AnyAsync(f => f.ModuleId == moduleId && f.NextStatusId == statusId && f.IsComplete, cancellationToken);
        }

        public async Task<bool> IsStartStatusAsync(int moduleId, byte statusId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AnyAsync(f => f.ModuleId == moduleId && f.NextStatusId == statusId && f.IsStart, cancellationToken);
        }

        public async Task<List<Status>> GetNextStatusesAsync(int moduleId, byte currentStatusId, CancellationToken cancellationToken = default)
        {
            // Get all flows from the current status
            var flows = await _dbSet
                .Where(f => f.ModuleId == moduleId && f.RequiredStatusId == currentStatusId)
                .Join(_context.Status,
                      flow => flow.NextStatusId,
                      status => status.Id,
                      (flow, status) => status)
                .ToListAsync(cancellationToken);

            // Separate cancelled status (Id == 3) if it exists
            var cancelled = flows.FirstOrDefault(s => s.Id == 3);
            if (cancelled != null)
            {
                flows.Remove(cancelled);
                flows.Add(cancelled); // Ensure Cancelled is last
            }

            return flows;
        }
    }
}
