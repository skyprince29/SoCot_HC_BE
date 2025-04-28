using SoCot_HC_BE.Model;
using System.Threading;
using System.Threading.Tasks;
using SoCot_HC_BE.Model.Requests;

public interface IHouseholdService
{
    Task SaveHouseholdAsync(SaveHouseholdRequest request, CancellationToken cancellationToken = default);
}
