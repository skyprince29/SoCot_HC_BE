using SoCot_HC_BE.DTO;
using SoCot_HC_BE.Model.Enums;

namespace SoCot_HC_BE.Dtos
{
    public class PatientRegistryDto : AuditInfoDto
    {
        public Guid PatientRegistryId { get; set; }

        public string? PatientRegistryCode { get; set; }

        public string? ReferralNo { get; set; }

        public Guid? PatientId { get; set; }

        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? Gender { get; set; }

        public string? ContactNumber { get; set; }

        public int? Age { get; set; }

        public bool IsTemporaryPatient { get; set; }

        public bool IsUrgent { get; set; }

        public PatientRegistryType PatientRegistryType { get; set; }

        public int FacilityId { get; set; }

        public bool IsActive { get; set; } = true;

        public byte? StatusId { get; set; }
    }
}
