using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    /// <summary>
    /// Provides logging services for user activities such as create, update, and status changes.
    /// </summary>
    public interface IActivityLogService : IRepository<ActivityLog, Guid>
    {
        /// <summary>
        /// Adds a create or update activity log to the database without saving changes immediately.
        /// </summary>
        /// <param name="userId">The ID of the user who performed the action.</param>
        /// <param name="isNew">Indicates whether the operation is a creation (true) or an update (false).</param>
        /// <param name="entityType">The name of the entity (e.g., "Patient", "RequestSlip").</param>
        /// <param name="entityId">The ID of the entity affected.</param>
        /// <param name="moduleId">The ID of the module where the action occurred.</param>
        /// <param name="message">An optional custom message to override the default.</param>
        /// <param name="link">An optional link to the entity in the UI.</param>
        Task AddAsync(Guid userId, bool isNew, string entityType, Guid entityId, int moduleId, string? message = null, string? link = null);

        /// <summary>
        /// Adds a status-change activity log to the database without saving changes immediately.
        /// </summary>
        /// <param name="userId">The ID of the user who performed the status change.</param>
        /// <param name="status">The new status (e.g., "Approved", "Cancelled").</param>
        /// <param name="entityType">The name of the entity (e.g., "RequestSlip").</param>
        /// <param name="entityId">The ID of the entity affected.</param>
        /// <param name="moduleId">The ID of the module where the status change occurred.</param>
        /// <param name="message">An optional custom message.</param>
        /// <param name="link">An optional link to the entity in the UI.</param>
        Task AddStatusAsync(Guid userId, string status, string entityType, Guid entityId, int moduleId, string? message = null, string? link = null);

        /// <summary>
        /// Adds a create or update activity log to the database and saves the changes immediately.
        /// </summary>
        /// <param name="userId">The ID of the user who performed the action.</param>
        /// <param name="isNew">Indicates whether the operation is a creation (true) or an update (false).</param>
        /// <param name="entityType">The name of the entity (e.g., "Patient", "RequestSlip").</param>
        /// <param name="entityId">The ID of the entity affected.</param>
        /// <param name="moduleId">The ID of the module where the action occurred.</param>
        /// <param name="message">An optional custom message to override the default.</param>
        /// <param name="link">An optional link to the entity in the UI.</param>
        Task AddAndSaveAsync(Guid userId, bool isNew, string entityType, Guid entityId, int moduleId, string? message = null, string? link = null);

        /// <summary>
        /// Adds a status-change activity log to the database and saves the changes immediately.
        /// </summary>
        /// <param name="userId">The ID of the user who performed the status change.</param>
        /// <param name="status">The new status (e.g., "Approved", "Cancelled").</param>
        /// <param name="entityType">The name of the entity (e.g., "RequestSlip").</param>
        /// <param name="entityId">The ID of the entity affected.</param>
        /// <param name="moduleId">The ID of the module where the status change occurred.</param>
        /// <param name="message">An optional custom message.</param>
        /// <param name="link">An optional link to the entity in the UI.</param>
        Task AddStatusLogAndSaveAsync(Guid userId, string status, string entityType, Guid entityId, int moduleId, string? message = null, string? link = null);
    }
}
