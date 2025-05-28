using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IPatientDepartmentTransactionService : IRepository<PatientDepartmentTransaction, Guid>
    {
        /// <summary>
        /// Retrieves a paginated list of PatientDepartmentTransactions filtered by department IDs,
        /// optional keyword, and optional status ID.
        /// </summary>
        /// <param name="fromDepartmentId">The originating department's ID.</param>
        /// <param name="currentDepartmentId">The current/receiving department's ID.</param>
        /// <param name="pageNo">Page number (1-based).</param>
        /// <param name="limit">Number of records per page.</param>
        /// <param name="keyword">Optional search keyword.</param>
        /// <param name="statusId">Optional status ID to filter results.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A paginated list of PatientDepartmentTransaction records.</returns>
        Task<List<PatientDepartmentTransaction>> GetAllWithPagingAsync(
            Guid fromDepartmentId,
            Guid currentDepartmentId,
            int pageNo,
            int limit,
            string? keyword = null,
            byte? statusId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Counts the number of PatientDepartmentTransactions filtered by department IDs,
        /// optional keyword, and optional status ID.
        /// </summary>
        /// <param name="fromDepartmentId">The originating department's ID.</param>
        /// <param name="currentDepartmentId">The current/receiving department's ID.</param>
        /// <param name="keyword">Optional search keyword.</param>
        /// <param name="statusId">Optional status ID to filter results.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>Total count of matching PatientDepartmentTransaction records.</returns>
        Task<int> CountAsync(
            Guid fromDepartmentId,
            Guid currentDepartmentId,
            string? keyword = null,
            byte? statusId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the AcceptedBy field for a specific PatientDepartmentTransaction.
        /// </summary>
        /// <param name="id">The ID of the transaction to update.</param>
        /// <param name="acceptedByUserId">The ID of the user accepting the transaction.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>True if update succeeded, false if the transaction was not found.</returns>
        Task<bool> UpdateAcceptedByAsync(
            Guid id,
            Guid acceptedByUserId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new patient department transaction by forwarding a patient
        /// from one department to another, and logs the forwarding action.
        /// </summary>
        /// <param name="dto">The data transfer object containing patient and department forwarding details.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation if needed.</param>
        /// <returns>The created <see cref="PatientDepartmentTransaction"/> object.</returns>
        Task<PatientDepartmentTransaction> CreateTransactionAsync(PatientForwardingDto dto, CancellationToken cancellationToken = default);
    }
}
