using SoCot_HC_BE.Model.BaseModels;
using SoCot_HC_BE.Model.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class Designation : AuditInfo
    {
        [Key]
        public Guid DesignationId { get; set; }
        [MaxLength(50)]
        public required string DesignationCode { get; set; }
        [MaxLength(300)]
        public required string DesignationName { get; set; }
        public bool IsActive { get; set; }
    }
}
