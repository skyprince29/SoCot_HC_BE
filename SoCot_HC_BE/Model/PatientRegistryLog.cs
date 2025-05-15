using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model
{
    public class PatientRegistryLog
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid PatientRegistryId { get; set; }

        [ForeignKey("PatientRegistryId")]
        public virtual PatientRegistry? PatientRegistry { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        public byte StatusId { get; set; }

        [ForeignKey("StatusId")]
        public virtual Status? Status { get; set; }

        public string? Remarks { get; set; }
    }
}
