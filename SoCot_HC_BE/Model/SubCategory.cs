using SoCot_HC_BE.Model.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model
{
    public class SubCategory : AuditInfo
    {
        [Key]
        public Guid SubCategoryId { get; set; }

        [MaxLength(50)]
        public required string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
