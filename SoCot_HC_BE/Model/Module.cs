using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model
{
    public class Module
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        public bool IsActive { get; set; } = true;
    }
}