using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;
using Route = SoCot_HC_BE.Model.Route;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IRouteService : IRepository<Route, Guid>
    {
        Task<List<Route>> GetAllActiveOnlyAsync(CancellationToken cancellationToken = default);
        Task<List<Route>> GetAllActiveWithCurrentAsync(Guid currentId, CancellationToken cancellationToken = default);
    }
}
