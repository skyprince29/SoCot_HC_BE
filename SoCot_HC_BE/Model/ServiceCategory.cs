using SoCot_HC_BE.Model.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class ServiceCategory : AuditInfo
    {
        [Key]
        public Guid ServiceCategoryId { get; set; }
        [MaxLength(100)]
        public required string ServiceCategoryName { get; set; }
        [MaxLength(200)]
        public string? Description { get; set; }
        public required int FacilityId { get; set; }

        [ForeignKey("FacilityId")]
        public Facility? Facility { get; set; }
        public bool IsActive { get; set; }
    }
}
