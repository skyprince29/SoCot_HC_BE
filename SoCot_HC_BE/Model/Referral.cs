using SoCot_HC_BE.Model.BaseModels;
using SoCot_HC_BE.Model.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class Referral : BaseTransactionWithAudit
    {
        public Referral()
        {
            base.ModuleId = (int)ModuleEnum.Referral;
        }
        [NotMapped]
        public override Guid TransactionId => ReferralId;

        [Key]
        public Guid ReferralId { get; set; }
        public long? TempRefId { get; set; }
        [Required, MaxLength(150)]
        public required string Complains { get; set; }
        [Required, MaxLength(150)]
        public required string Reason { get; set; }
        [MaxLength(150)]
        public string? Remarks { get; set; }
        [MaxLength(150)]
        public string? Diagnosis { get; set; }
        [Required]
        public required int ReferredTo { get; set; }
        [ForeignKey("ReferredTo")]
        public Facility? FacilityReferredTo { get; set; }
        [Required]
        public required int ReferredFrom { get; set; }
        [ForeignKey("ReferredFrom")]
        public Facility? FacilityReferredFrom { get; set; }
        [Required, MaxLength(50)]
        public required string ReferralNo { get; set; }
        [Required]
        public DateTime ReferralDateTime { get; set; }
        [MaxLength(150)]
        public string? DischargeInstructions { get; set; }
        public Guid? PersonnelId { get; set; }
        [ForeignKey("PersonnelId")]
        public Personnel? Personnel { get; set; }
        public Guid? AttendingPhysicianId { get; set; }
        [ForeignKey("AttendingPhysicianId")]
        public Personnel? AttendingPhysician { get; set; }
        public bool IsUrgent { get; set; }
        [ForeignKey("StatusId")]
        public Status? Status { get; set; }

        // Navigation property
        public virtual ICollection<ReferralService> ReferralServices { get; set; }
    }
}
