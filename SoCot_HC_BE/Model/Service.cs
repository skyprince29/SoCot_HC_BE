using SoCot_HC_BE.Model.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class Service : AuditInfo
    {
        [Key]
        public Guid ServiceId { get; set; }

        [MaxLength(50)]
        public required string ServiceName { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }

        public required int FacilityId { get; set; }

        [ForeignKey("FacilityId")]
        public Facility? Facility { get; set; }

        public required Guid DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }

        public required int ServiceClassificationId { get; set; }

        [ForeignKey("ServiceClassificationId")]
        public virtual ServiceClassification? ServiceClassification { get; set; }
        public required Guid ServiceCategoryId { get; set; }

        [ForeignKey("ServiceCategoryId")]
        public virtual ServiceCategory? ServiceCategory { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<ServiceDepartment> ServiceDepartments { get; set; }
    }
}
