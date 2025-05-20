using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model
{
    public class TransactionFlowHistory
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [Required]
        public int ModuleId { get; set; }

        [ForeignKey("ModuleId")]
        public virtual Module? Module { get; set; }

        [Required]
        public byte PreviousStatusId { get; set; }

        [ForeignKey("PreviousStatusId")]
        public virtual Status? PreviousStatus { get; set; }

        [Required]
        public byte CurrentStatusId { get; set; }

        [ForeignKey("CurrentStatusId")]
        public virtual Status? CurrentStatus { get; set; }

        [Required]
        public Guid TransactionId { get; set; }  // Unique ID of the related transaction

        public string? Remarks { get; set; }

        public bool IsComplete { get; set; }
    }
}
