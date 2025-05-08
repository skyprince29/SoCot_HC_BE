using SoCot_HC_BE.Model.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model
{
    public class Product : AuditInfo
    {
        [Key]
        public Guid ProductId { get; set; }  

        [MaxLength(50)]
        public required string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
