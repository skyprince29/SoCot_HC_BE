using SoCot_HC_BE.Model;
using System.Threading;
using System.Threading.Tasks;
using SoCot_HC_BE.Model.Requests;
using SoCot_HC_BE.Repositories.Interfaces;
using SoCot_HC_BE.DTO;

public interface IHouseholdService : IRepository<Household, Guid>
{
    // Get a list of Household with paging, using CancellationToken for async cancellation support.
    Task<List<Household>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default);

    // Get the total count of Household, again supporting async cancellation.
    Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default);

    // Save Household
    Task SavePersonDetails(SaveHouseholdRequest request, CancellationToken cancellationToken = default);

    Task AppendFamilyToExistingHousehold(AppendFamilyRequest request, CancellationToken cancellationToken = default);

    Task AppendMemberToExistingFamily(AppendMemberRequest request, CancellationToken cancellationToken = default);

}
