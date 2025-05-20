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

        public byte? RequiredStatusId { get; set; }  // nullable for starting status

        [ForeignKey("RequiredStatusId")]
        public virtual Status? RequiredStatus { get; set; }

        [Required]
        public byte NextStatusId { get; set; }

        [ForeignKey("NextStatusId")]
        public virtual Status NextStatus { get; set; } = null!;

        public bool IsStart { get; set; } = false;      // Mark if this flow starts here

        public bool IsComplete { get; set; } = false;   // Mark if this flow ends here
    }
}
