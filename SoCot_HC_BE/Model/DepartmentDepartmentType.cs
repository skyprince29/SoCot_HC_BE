using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model
{
    public class DepartmentDepartmentType
    {
        [Key]
        public Guid DepartmentDepartmentTypeId { get; set; }

        public Guid DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department? Department { get; set; }

        public Guid DepartmentTypeId { get; set; }

        [ForeignKey("DepartmentTypeId")]
        public virtual DepartmentType? DepartmentType { get; set; }
    }
}
