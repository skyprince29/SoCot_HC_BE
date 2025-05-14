using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.DTO
{
    public abstract class AuditInfoDto
    {
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
