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

        [MaxLength(25)]
        public required string PatientRegistryCode { get; set; }
        [MaxLength(25)]
        public string? ReferralNo { get; set; }

        public Guid? PatientId { get; set; } // Foreign Key for Patient

        [MaxLength(50)]
        public required string Name { get; set; }
        [MaxLength(50)]
        public string? Address { get; set; }

        [MaxLength(20)]
        [EnumDataType(typeof(Gender))] // For validation only
        public required string Gender { get; set; } // Stored as string
        [MaxLength(15)]
        public string? ContactNumber { get; set; }

        public int? Age { get; set; }

        public bool IsTemporaryPatient { get; set; }
        [MaxLength(20)]
        public required PatientRegistryType PatientRegistryType { get; set; } // Enum stored as int
    }
}
