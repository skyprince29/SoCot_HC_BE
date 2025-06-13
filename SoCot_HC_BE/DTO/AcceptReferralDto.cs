using SoCot_HC_BE.Model.Enums;

namespace SoCot_HC_BE.DTO
{
    public class AcceptReferralDto
    {
        public int facilityId { get; set; }
        public Guid serviceId { get; set; }
        public string? referralNo { get; set; }
        public Guid patientId { get; set; }
        public PatientRegistryType patientRegistryType { get; set; }
        public bool isUrgent { get; set; }
    }
}
