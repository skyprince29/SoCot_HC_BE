using SoCot_HC_BE.Model.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class Referral : AuditInfo
    {
        [Key]
        public Guid ReferralId { get; set; }
        public long? TempRefId { get; set; }
        [MaxLength(150)]
        public string? Complains { get; set; }
        [MaxLength(150)]
        public string? Reason { get; set; }
        [MaxLength(150)]
        public string? Remarks { get; set; }
        [MaxLength(150)]
        public string? Diagnosis { get; set; }
        [MaxLength(100)]
        public string? Status { get; set; }
        [Required]
        public int ReferredTo { get; set; }
        public Facility? FacilityReferredTo { get; set; }
        [Required]
        public int ReferredFrom { get; set; }
        public Facility? FacilityReferredFrom { get; set; }
        [Required]
        [MaxLength(50)]
        public string? ReferralNo { get; set; }
        [Required]
        public DateTime ReferralDateTime { get; set; }
        public DateTime? ArrivalDateTime { get; set; }
        public DateTime? AdmissionDateTime { get; set; }
        public DateTime? DischargeDateTime { get; set; }
        [MaxLength(150)]
        public string? DischargeInstructions { get; set; }
        public Guid? PersonnelId { get; set; }
        [ForeignKey("PersonnelId")]
        public Personnel? Personnel { get; set; }
        public Guid? AttendingPhysicianId { get; set; }
        [ForeignKey("AttendingPhysicianId")]
        public Personnel? AttendingPhysician { get; set; }
        public bool IsAccepted { get; set; }
        public bool isAlreadyUse { get; set; }
        public long ReferrenceId { get; set; }
        public bool IsUrgent { get; set; }

        // Navigation property
        public virtual ICollection<ReferralService>? ReferralServices { get; set; }
    }
}
