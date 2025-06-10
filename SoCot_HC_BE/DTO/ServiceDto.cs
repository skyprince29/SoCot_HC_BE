using SoCot_HC_BE.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.DTO
{
    public class ServiceDto :AuditInfoDto
    {
        public Guid ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public string? Description { get; set; }
        public int FacilityId { get; set; }
        public Guid DepartmentId { get; set; }
        public int ServiceClassificationId { get; set; }
        public Guid ServiceCategoryId { get; set; }
        public bool IsActive { get; set; }
        public List<Guid> DepartmentIds { get; set; }
    }
}
