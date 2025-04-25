using SoCot_HC_BE.Model.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model
{
    public class DepartmentType :AuditInfo
    {
        [Key]
        public Guid DepartmentTypeId { get; set; }
        [MaxLength(50)]
        public required string DepartmentTypeName { get; set; }
        [MaxLength(300)]
        public string? Description { get; set; }
        public bool IsActive { get; set; }

    }
}
