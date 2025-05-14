using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SoCot_HC_BE.Model.BaseModels;

namespace SoCot_HC_BE.Model
{
    public class SupplyStorage : AuditInfo
    {
        [Key]
        public Guid SupplyStorageId { get; set; }

        [MaxLength(50)]
        public required string SupplyStorageName { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }

        public required int FacilityId { get; set; }

        [ForeignKey("FacilityId")]
        public Facility? Facility { get; set; }

        public required Guid DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }

        public bool IsActive { get; set; }
    }
}
