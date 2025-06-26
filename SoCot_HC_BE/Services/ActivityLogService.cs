using SoCot_HC_BE.Data;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories;
using SoCot_HC_BE.Services.Interfaces;

namespace SoCot_HC_BE.Services
{
    public class ActivityLogService : Repository<ActivityLog, Guid>, IActivityLogService
    {


        public ActivityLogService(AppDbContext context) : base(context)
        {
        }

        // --- Create/Update + Save ---
        public async Task AddAndSaveAsync(
            Guid userId,
            bool isNew,
            string entityType,
            Guid entityId,
            int moduleId,
            string? message = null,
            string? link = null)
        {
            var log = CreateStandardLog(userId, isNew, entityType, entityId, moduleId, message, link);
            await _dbSet.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        // --- Status Change + Save ---
        public async Task AddStatusLogAndSaveAsync(
            Guid userId,
            string status,
            string entityType,
            Guid entityId,
            int moduleId,
            string? message = null,
            string? link = null)
        {
            var log = CreateStatusLog(userId, status, entityType, entityId, moduleId, message, link);
            await _dbSet.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        // --- Create/Update (No Save) ---
        public async Task AddAsync(
            Guid userId,
            bool isNew,
            string entityType,
            Guid entityId,
            int moduleId,
            string? message = null,
            string? link = null)
        {
            var log = CreateStandardLog(userId, isNew, entityType, entityId, moduleId, message, link);
            await _dbSet.AddAsync(log);
        }

        // --- Status Change (No Save) ---
        public async Task AddStatusAsync(
            Guid userId,
            string status,
            string entityType,
            Guid entityId,
            int moduleId,
            string? message = null,
            string? link = null)
        {
            var log = CreateStatusLog(userId, status, entityType, entityId, moduleId, message, link);
            await _dbSet.AddAsync(log);
        }

        // --- Internal: Builds standard log message ---
        private ActivityLog CreateStandardLog(
            Guid userId,
            bool isNew,
            string entityType,
            Guid entityId,
            int moduleId,
            string? message,
            string? link)
        {
            string action = isNew ? "Created" : "Updated";

            string defaultMessage = message ?? (isNew
                ? $"{entityType} successfully created."
                : $"{entityType} successfully updated.");

            return BuildLog(userId, action, entityType, entityId, moduleId, defaultMessage, link);
        }

        // --- Internal: Builds status log message ---
        private ActivityLog CreateStatusLog(
            Guid userId,
            string status,
            string entityType,
            Guid entityId,
            int moduleId,
            string? message,
            string? link)
        {
            string defaultMessage = message ?? $"{entityType} status updated to {status}.";
            return BuildLog(userId, status, entityType, entityId, moduleId, defaultMessage, link);
        }

        // --- Internal: Core log creation ---
        private ActivityLog BuildLog(
            Guid userId,
            string action,
            string entityType,
            Guid entityId,
            int moduleId,
            string message,
            string? link)
        {
            return new ActivityLog
            {
                ActivityLogId = Guid.NewGuid(),
                UserId = userId,
                Action = action,
                EntityType = entityType,
                EntityId = entityId,
                ModuleId = moduleId,
                Message = message,
                Link = link ?? string.Empty,
                Timestamp = DateTime.Now
            };
        }
    }
}
