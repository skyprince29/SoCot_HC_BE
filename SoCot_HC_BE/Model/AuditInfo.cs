using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model
{
    public abstract class AuditInfo
    {
        [Required]
        public long CreatedBy { get; set; }
        //[NotMapped]
        //public UserAccount CreatedByUser { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        public long? UpdatedBy { get; set; }
        //[NotMapped]
        //public UserAccount UpdatedByUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
