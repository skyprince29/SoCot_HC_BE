using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model.BaseModels
{
    public abstract class AuditInfo
    {
        [Required]
        public Guid CreatedBy { get; set; }
        //[NotMapped]
        //public UserAccount CreatedByUser { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        //[NotMapped]
        //public UserAccount UpdatedByUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
