using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model
{
    public class ItemCategory
    {
        [Key]
        public Guid ItemCategoryId { get; set; }

        [MaxLength(50)]
        public required string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
