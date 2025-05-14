using SoCot_HC_BE.Model.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class DentalRecord : AuditInfo
    {
        public Guid DentalRecordId { get; set; }

        [ForeignKey("Person")]
        public Guid? PatientId { get; set; }
        public Person? Patient { get; set; }

        [ForeignKey("Facility")]
        public int? FacilityId { get; set; }
        public Facility? Facility { get; set; }

        [ForeignKey("DentalRecordDetailsMedicalHistory")]
        public Guid? DentalRecordDetailsMedicalHistoryId { get; set; }
        public DentalRecordDetailsMedicalHistory? DentalRecordDetailsMedicalHistory { get; set; }


        [ForeignKey("DentalRecordDetailsSocialHistory")]
        public Guid? DentalRecordDetailsSocialHistoryId { get; set; }
        public DentalRecordDetailsSocialHistory? DentalRecordDetailsSocialHistory { get; set; }


        [ForeignKey("DentalRecordDetailsOralHealthCondition")]
        public Guid? DentalRecordDetailsOralHealthConditionId { get; set; }
        public DentalRecordDetailsOralHealthCondition? DentalRecordDetailsOralHealthCondition { get; set; }


        [ForeignKey("DentalRecordDetailsPersence")]
        public Guid? DentalRecordDetailsPersenceId { get; set; }
        public DentalRecordDetailsPresence? DentalRecordDetailsPresence { get; set; }


        [ForeignKey("DentalRecordDetailsToothCount")]
        public Guid? DentalRecordDetailsToothCountId { get; set; }
        public DentalRecordDetailsToothCount? DentalRecordDetailsToothCount { get; set; }

        public virtual ICollection<DentalRecordDetailsServices>? DentalRecordDetailsServices { get; set; }
        public virtual ICollection<DentalRecordDetailsFindings>? DentalRecordDetailsFindings { get; set; }

        //[ForeignKey("Person")]
        public String ConsentedByName { get; set; } = string.Empty;


        [ForeignKey("Person")]
        public Guid? PhysicianId { get; set; }
        public Person? Physician { get; set; }

        public Guid ReferralId { get; set; }
        public String ReferralNo { get; set; } = string.Empty;
        public DateTime DateRecord { get; set; }


    }
}
