namespace SoCot_HC_BE.DTO
{
    public class FacilityReferralDto : AuditInfoDto
    {
        public Guid ReferralId { get; set; }
        public long? TempRefId { get; set; }
        public string? Complains { get; set; }
        public string? Reason { get; set; }
        public string? Remarks { get; set; }
        public string? Diagnosis { get; set; }
        public int? ReferredTo { get; set; }
        public int? ReferredFrom { get; set; }
        public string? ReferralNo { get; set; }
        public DateTime? ReferralDateTime { get; set; }
        public string? DischargeInstructions { get; set; }
        public Guid? PersonnelId { get; set; }
        public Guid? AttendingPhysicianId { get; set; }
        public bool IsUrgent { get; set; }
        public byte? StatusId { get; set; }


        // List of ReferralService IDs
        public List<Guid> ReferralServiceIds { get; set; }
    }
}
