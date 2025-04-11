using SoCot_HC_BE.Model;
using SoCot_HC_BE.Repositories.Intefaces;
using SoCot_HC_BE.Services.Interfaces;

namespace SoCot_HC_BE.Services
{
    public class VitalSignService : IVitalSignService
    {
        private readonly IRepository<VitalSign, long> _repository;

        // Injecting the generic repository for CRUD operations
        public VitalSignService(IRepository<VitalSign, long> repository)
        {
            _repository = repository;
        }

        // CRUD Operations
        public async Task<List<VitalSign>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<VitalSign?> GetAsync(long id) => await _repository.GetAsync(id);
        public async Task AddAsync(VitalSign entity) => await _repository.AddAsync(entity);
        public async Task UpdateAsync(VitalSign entity) => await _repository.UpdateAsync(entity);
        public async Task DeleteAsync(long id) => await _repository.DeleteAsync(id);

        // Pagination & Filtering (added methods)
        public async Task<List<VitalSign>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null)
        {
            // Fetch the list of VitalSigns from the repository
            var vitalSigns = await GetAllAsync();

            // Apply filtering based on the keyword, if provided
            if (!string.IsNullOrEmpty(keyword))
            {
                /*vitalSigns = vitalSigns.Where(v => v.Name.Contains(keyword)).ToList();*/ // Adjust filtering as needed
            }

            // Apply pagination (skip and take)
            return vitalSigns.Skip((pageNo - 1) * limit).Take(limit).ToList();
        }

        // Count total records for pagination
        public async Task<int> CountAsync(string? keyword = null)
        {
            // Fetch the list of VitalSigns from the repository
            var vitalSigns = await GetAllAsync();

            // Apply filtering based on the keyword, if provided
            if (!string.IsNullOrEmpty(keyword))
            {
                /*vitalSigns = vitalSigns.Where(v => v.Name.Contains(keyword)).ToList();*/ // Adjust filtering as needed
            }

            // Return the count of records
            return vitalSigns.Count();
        }
    }

}
