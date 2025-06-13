using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface ISchoolAgeProfileService : IRepository<SchoolAgeProfile, Guid>
    {
        Task<List<SchoolAgeProfile>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default);
        Task SaveSchoolAgeProfileAsync(SchoolAgeProfileDto schoolageprofile, CancellationToken cancellationToken = default);
        Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default);
    }
}
