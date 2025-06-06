using SoCot_HC_BE.Model.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class UserDepartment : AuditInfo
    {
        [Key]
        public Guid UserDepartmentId { get; set; }
        public Guid? PersonId { get; set; }
        public virtual Person? Person { get; set; }

        public Guid? DepartmentId { get; set; }
        public virtual Department? Department { get; set; }

        public bool IsActive { get; set; }
    }
}
