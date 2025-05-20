using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model
{
    public class ModuleStatusFlow
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ModuleId { get; set; }

        [ForeignKey("ModuleId")]
        public virtual Module Module { get; set; } = null!;

        [Required]
        public byte RequiredStatusId { get; set; }  // i.e., FROM this status

        [ForeignKey("RequiredStatusId")]
        public virtual Status RequiredStatus { get; set; } = null!;

        [Required]
        public byte NextStatusId { get; set; }  // i.e., TO this status

        [ForeignKey("NextStatusId")]
        public virtual Status NextStatus { get; set; } = null!;
    }
}
