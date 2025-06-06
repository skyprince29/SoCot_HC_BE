using SoCot_HC_BE.Dtos;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Model.Enums;
using SoCot_HC_BE.Repositories.Interfaces;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IPatientRegistryService : IRepository<PatientRegistry, Guid>
    {
        // Get a list of VitalSigns with paging, using CancellationToken for async cancellation support.
        Task<List<PatientRegistry>> GetAllWithPagingAsync(int pageNo, int limit, string? keyword = null, CancellationToken cancellationToken = default);

        // Get the total count of VitalSigns, again supporting async cancellation.
        Task<int> CountAsync(string? keyword = null, CancellationToken cancellationToken = default);

        //Save Patient Registry
        Task<PatientRegistry> SavePatientRegistryAsync(PatientRegistryDto patientRegistryDto, CancellationToken cancellationToken = default);
        // Create a new Patient Registry
        Task<PatientRegistry> CreatePatientRegistryAsync(string? referralNo, Guid patientId, PatientRegistryType patientRegistryType, int facilityId, bool isUrgent = false, CancellationToken cancellationToken  = default);
    }
}
