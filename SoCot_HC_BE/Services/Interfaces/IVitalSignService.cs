using SoCot_HC_BE.Model;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IVitalSignService
    {
        Task<List<VitalSign>> GetAllAsync();
        Task<VitalSign?> GetAsync(long id);
        Task AddAsync(VitalSign entity);
        Task UpdateAsync(VitalSign entity);
        Task DeleteAsync(long id);
        Task<List<VitalSign>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null);
        Task<int> CountAsync(string? keyword = null);
    }

}
