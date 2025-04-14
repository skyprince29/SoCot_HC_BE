using SoCot_HC_BE.Model.BaseModels;
using SoCot_HC_BE.Model.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class PatientRegistry : AuditInfo
    {
        [Key]
        public Guid PatientRegistryId { get; set; }
        public required string PatientRegistryCode { get; set; }
        public string? ReferralNo { get; set; }
        public Guid? PatientId { get; set; } // Foreign Key for Patient
        public required string Name { get; set; }
        public string? Address { get; set; }
        public required string Gender { get; set; }
        public string? ContactNumber { get; set; }
        public int? Age { get; set; }
        public bool IsTemporaryPatient { get; set; }
        public PatientRegistryType PatientRegistryType { get; set; }
    }
}
