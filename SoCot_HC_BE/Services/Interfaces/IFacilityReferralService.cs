using SoCot_HC_BE.DTO;
using SoCot_HC_BE.DTO.OldReferralDto;
using SoCot_HC_BE.DTO.ParamDto;
using SoCot_HC_BE.Dtos;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IFacilityReferralService : IRepository<Referral, Guid>
    {
        /// <summary>
        /// Retrieves a paginated list of Referral filtered by department IDs,
        /// transaction date range, optional keyword, and optional status ID.
        /// </summary>
        /// <param name="request">Pagination and filter parameters.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A paginated list of PatientDepartmentTransaction records.</returns>
        //Task<List<Referral>> GetAllWithPagingAsync(
        //    GetPagedReferralParam request,
        //    CancellationToken cancellationToken = default);

        /// <summary>
        /// Counts the number of Referral filtered by department IDs,
        /// transaction date range, optional keyword, and optional status ID.
        /// </summary>
        /// <param name="request">Pagination and filter parameters.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>Total count of matching PatientDepartmentTransaction records.</returns>
        //Task<int> CountAsync(
        //    GetPagedReferralParam request,
        //    CancellationToken cancellationToken = default);

        //Save Referral
        Task<Referral> SaveReferralAsync(FacilityReferralDto facilityReferralDto, CancellationToken cancellationToken = default);
    }
}
