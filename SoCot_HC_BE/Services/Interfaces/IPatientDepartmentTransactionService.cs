using SoCot_HC_BE.DTO;
using SoCot_HC_BE.DTO.ParamDto;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IPatientDepartmentTransactionService : IRepository<PatientDepartmentTransaction, Guid>
    {
        /// <summary>
        /// Retrieves a paginated list of PatientDepartmentTransactions filtered by department IDs,
        /// transaction date range, optional keyword, and optional status ID.
        /// </summary>
        /// <param name="request">Pagination and filter parameters.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A paginated list of PatientDepartmentTransaction records.</returns>
        Task<List<PatientDepartmentTransaction>> GetAllWithPagingAsync(
            GetPagedPDTRequestParam request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Counts the number of PatientDepartmentTransactions filtered by department IDs,
        /// transaction date range, optional keyword, and optional status ID.
        /// </summary>
        /// <param name="request">Pagination and filter parameters.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>Total count of matching PatientDepartmentTransaction records.</returns>
        Task<int> CountAsync(
            GetPagedPDTRequestParam request,
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
