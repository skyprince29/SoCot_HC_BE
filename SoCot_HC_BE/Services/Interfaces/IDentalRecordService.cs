using SCHC_API.Handler;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IDentalRecordService : IRepository<DentalRecord, Guid>
    {
        Task<PaginationHandler<DentalRecord>> GetAllWithPagingAsync(int pageNo, int limit, string keyword = "", CancellationToken cancellationToken = default);
        Task<DentalRecord> CreateDentalRecord(string ReferralNo, CancellationToken cancellationToken = default);
        Task<DentalRecord> UpdateDentalRecord(Guid DentalRecordId, CancellationToken cancellationToken = default);
        Task SaveOrUpdateDentalRecordAsync(DentalDTO.DentalRecordDTO dentalRecord, CancellationToken cancellationToken = default);
        Task<DentalRecord> GetDentalRecord(Guid dentalRecordId, CancellationToken cancellationToken = default);

    }
}
