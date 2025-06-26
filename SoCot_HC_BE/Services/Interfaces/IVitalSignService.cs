using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model;
using SoCot_HC_BE.Model.Enums;
using SoCot_HC_BE.Repositories.Interfaces;

public interface IVitalSignService : IRepository<VitalSign, Guid>
{
    // Get VitalSignDTO
    Task<VitalSignDto?> GetVitalSignDtoAsync(Guid id, CancellationToken cancellationToken = default);

    // Get a list of VitalSigns with paging and filtering
    Task<List<VitalSignDto>> GetAllWithPagingAsync(
        int pageNo,
        int limit,
        VitalSignReferenceType? referenceType = null,
        Guid? referenceId = null,
        CancellationToken cancellationToken = default);

    // Get the total count of VitalSigns with optional filters
    Task<int> CountAsync(
        VitalSignReferenceType? referenceType = null,
        Guid? referenceId = null,
        CancellationToken cancellationToken = default);

    // Save or update VitalSign and its references
    Task SaveVitalSignAsync(VitalSignDto vitalSignDto, CancellationToken cancellationToken = default);
    Task SaveVitalSignAsync(VitalSignDto vitalSignDto, bool isReferrencesaving, CancellationToken cancellationToken = default);

    VitalSign DTOToModel(VitalSignDto dto);

    void ValidateFields(VitalSignDto vitalSignDto, Dictionary<string, List<string>>? preErrors, string prefix = "");
}
