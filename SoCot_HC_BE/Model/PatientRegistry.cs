using SoCot_HC_BE.Model.BaseModels;
using SoCot_HC_BE.Model.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class PatientRegistry : BaseTransactionWithAudit
    {
        public PatientRegistry()
        {
            base.ModuleId = (int)ModuleEnum.PatientRegistry;
        }

        [NotMapped]
        public override Guid TransactionId => PatientRegistryId;

        [Key]
        public Guid PatientRegistryId { get; set; }

        [MaxLength(25)]
        public required string PatientRegistryCode { get; set; }
        [MaxLength(25)]
        public string? ReferralNo { get; set; }

        public Guid? PatientId { get; set; } // Foreign Key for Person

        [MaxLength(50)]
        public required string Name { get; set; }
        [MaxLength(200)]
        public string? Address { get; set; }

        [MaxLength(20)]
        [EnumDataType(typeof(Gender))] // For validation only
        public required string Gender { get; set; } // Stored as string
        [MaxLength(15)]
        public string? ContactNumber { get; set; }

        public int? Age { get; set; }

        public bool IsTemporaryPatient { get; set; }

        public bool IsUrgent { get; set; }

        public required PatientRegistryType PatientRegistryType { get; set; } // Enum stored as int

        //Need to implement this if there is already a user table
        //One-to-one with Facility(int FK)
        public int FacilityId { get; set; }

        [ForeignKey("FacilityId")]
        public Facility? Facility { get; set; }

        public bool IsActive { get; set; } = true;

        [ForeignKey("StatusId")]
        public Status? Status { get; set; }

        public Guid? ServiceId { get; set; }

        [ForeignKey("ServiceId")]
        public Service? Service { get; set; }

        [NotMapped]
        public bool IsForwarded { get; set; }
    }
}
