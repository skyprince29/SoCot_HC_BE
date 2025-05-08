using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model
{
    public class Status
    {
        [Key]
        public byte Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        public bool IsActive { get; set; } = true;
    }
}