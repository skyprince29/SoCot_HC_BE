using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model
{
    public class DentalRecordDetailsMedicalHistory
    {
        public Guid DentalRecordDetailsMedicalHistoryId { get; set; }
        public Boolean HasAlergies { get; set; }
        [MaxLength(500)]
        public String Alergies { get; set; } = string.Empty;
        public Boolean HasHypertentionOrCVA { get; set; }
        public Boolean HasDiabetesMelitus { get; set; }
        public Boolean HasBloodDisorders { get; set; }
        public Boolean HasCardiovascularOrHeartDiseases { get; set; }
        public Boolean HasThyroidDisorders { get; set; }
        public Boolean HasHepatitis { get; set; }
        [MaxLength(500)]
        public String HepatitisType { get; set; } = string.Empty;
        public Boolean HasMalignancy { get; set; }
        [MaxLength(500)]
        public String MalignancyType { get; set; } = string.Empty;
        public Boolean HasHistoryOfPrevHospitalization { get; set; }
        [MaxLength(500)]
        public String Medical { get; set; } = string.Empty;
        [MaxLength(500)]
        public String Surgical { get; set; } = string.Empty;
        public Boolean HasBloodTransfusion { get; set; }
        public int? BloodTransfusionMonth { get; set; }
        public int? BloodTransfusionYear { get; set; }
        public Boolean HasTattoo { get; set; }
        public Boolean HasOthers { get; set; }
        [MaxLength(500)]
        public String Others { get; set; } = string.Empty;
    }
}
