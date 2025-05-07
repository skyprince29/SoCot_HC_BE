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
        [Required]
        public  string DepartmentCode { get; set; }
        [Required]
        public int FacilityId { get; set; }
        [ForeignKey("FacilityId")]
        public  Facility? Facility { get; set; }
        [MaxLength(100)]
        [Required]
        public  string DepartmentName { get; set; }
        [MaxLength(200)]
        [Required]
        public string? Description { get; set; }
        public Guid? ParentDepartmentId { get; set; }
        [ForeignKey("ParentDepartmentId")]
        public virtual Department? ParentDepartment { get; set; }
        // "Can Receive Patient Referrals" refers to referral by department
        public bool IsReferable { get; set; }
        public bool IsActive { get; set; }

        // Navigation property
        public  virtual ICollection<DepartmentDepartmentType> DepartmentTypes { get; set; }
    }
}
