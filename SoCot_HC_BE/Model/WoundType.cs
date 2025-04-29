using SoCot_HC_BE.Model.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model
{
    public class WoundType : AuditInfo
    {
        [Key]
        public int WoundTypeId { get; set; }
        [MaxLength(100)]
        public required string WoundTypeName { get; set; }
        [MaxLength(200)]
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
