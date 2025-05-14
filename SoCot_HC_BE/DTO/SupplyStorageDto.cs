using SoCot_HC_BE.Model;

namespace SoCot_HC_BE.DTO
{
    public class SupplyStorageDto : AuditInfoDto
    {
        public Guid SupplyStorageId { get; set; }
        public string? SupplyStorageName { get; set; }
        public string? Description { get; set; }
        public int FacilityId { get; set; }
        public Guid DepartmentId { get; set; }
        public bool IsActive { get; set; }
    }
}
