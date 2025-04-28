using SoCot_HC_BE.Model.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class Department : AuditInfo
    {
        [Key]
        public Guid DepartmentId { get; set; }
        [MaxLength(50)]
        public required string DepartmentCode { get; set; }
        public required int FacilityId { get; set; }
        [ForeignKey("FacilityId")]
        public virtual Facility? Facility { get; set; }
        [MaxLength(100)]
        public required string DepartmentName { get; set; }
        [MaxLength(200)]
        public string? Description { get; set; }
        public Guid? ParentDepartmentId { get; set; }
        [ForeignKey("ParentDepartmentId")]
        public virtual Department? ParentDepartment { get; set; }
        // "Can Receive Patient Referrals" refers to referral by department
        public bool IsReferable { get; set; }
        public bool IsActive { get; set; }

        // Navigation property
        public required virtual ICollection<DepartmentDepartmentType> DepartmentTypes { get; set; }
    }
}
