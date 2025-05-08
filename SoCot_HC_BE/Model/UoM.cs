using SoCot_HC_BE.Model.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model
{
    public class UoM : AuditInfo
    {
        [Key]
        public Guid UoMId { get; set; }

        [MaxLength(10)]
        public required string Abbreviation { get; set; }

        [MaxLength(50)]
        public required string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
