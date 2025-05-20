using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface ISupplyStorageService : IRepository<SupplyStorage, Guid>
    {
        // Get a list of Supply Storage with paging, using CancellationToken for async cancellation support.
        Task<List<SupplyStorage>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, Guid? departmentId = null, int? facilityId = null,  CancellationToken cancellationToken = default);

        // Get the total count of Supply Storages, again supporting async cancellation.
        Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default);

        Task<SupplyStorageDto> GetSupplyStorageDtoAsync(Guid? SupplyStorageId);

        Task SaveSupplyStorageAsync(SupplyStorageDto supplyStorageDto, CancellationToken cancellationToken = default);
    }
}
