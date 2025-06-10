using SoCot_HC_BE.Model.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class ServiceDepartment
    {
        [Key]
        public Guid ServiceDepartmentId { get; set; }
        [Required]
        public Guid ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public virtual Service? Service { get; set; }

        [Required]
        public Guid DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual Department? Department { get; set; }

        public bool IsActive { get; set; }
    }
}
