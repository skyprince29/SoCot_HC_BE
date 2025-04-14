using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

public interface IVitalSignService : IRepository<VitalSign, Guid>
{
    // Get a list of VitalSigns with paging, using CancellationToken for async cancellation support.
    Task<List<VitalSign>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default);

    // Get the total count of VitalSigns, again supporting async cancellation.
    Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default);

    // This method could still be useful, but ideally, you want everything to use CancellationToken.
    // If you're doing a method without CancellationToken, it's best to reconsider whether it's needed.
    Task<List<VitalSign>> GetAllWithoutTokenAsync();
}
