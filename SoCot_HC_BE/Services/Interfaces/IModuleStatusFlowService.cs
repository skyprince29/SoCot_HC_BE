using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IModuleStatusFlowService : IRepository<ModuleStatusFlow, int>
    {
        /// <summary>
        /// Gets all status flows for the specified module.
        /// </summary>
        Task<List<ModuleStatusFlow>> GetAllStatusFlowsAsync(int moduleId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all statuses for the specified module.
        /// </summary>
        Task<List<Status>> GetAllStatusesByModuleAsync(int moduleId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the first (start) status for the specified module.
        /// </summary>
        Task<Status?> GetFirstStatusAsync(int moduleId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all possible next statuses from the current status in the module.
        /// </summary>
        Task<List<Status>> GetNextStatusesAsync(int moduleId, byte currentStatusId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if the specified status is a start status for the module.
        /// </summary>
        Task<bool> IsStartStatusAsync(int moduleId, byte statusId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if the specified status is a complete status for the module.
        /// </summary>
        Task<bool> IsCompleteStatusAsync(int moduleId, byte statusId, CancellationToken cancellationToken = default);
    }
}
