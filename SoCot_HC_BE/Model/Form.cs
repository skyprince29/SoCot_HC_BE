using SoCot_HC_BE.Model.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model
{
    public class Form : AuditInfo
    {
        [Key]
        public Guid FormId { get; set; }

        [MaxLength(10)]
        public required string Code { get; set; }

        [MaxLength(50)]
        public required string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
