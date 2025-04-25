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
        public virtual required Facility Facility { get; set; }
        [MaxLength(100)]
        public required string DepartmentName { get; set; }
        public Guid? ParentDepartmentId { get; set; }
        [ForeignKey("ParentDepartmentId")]
        public virtual required Department? ParentDepartment { get; set; }
        // "Can Receive Patient Referrals" refers to referral by department
        public bool IsReferable { get; set; }
        public bool IsActive { get; set; }
    }
}
