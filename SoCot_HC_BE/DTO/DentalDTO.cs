using SoCot_HC_BE.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.DTO
{
    public class DentalDTO
    {

        public class DentalRecordDTO : AuditInfoDto
        {
            public Guid DentalRecordId { get; set; }
            public Guid PatientId { get; set; }
            public int FacilityId { get; set; }
            public Guid PhysicianId { get; set; }
            public Guid PatientRegistryId { get; set; }
            public String ReferralNo { get; set; } = string.Empty;
            public String ConsentedByName { get; set; } = string.Empty;
            public DateTime DateRecord { get; set; }

            public MedicalHistoryDTO MedicalHistory { get; set; } = new MedicalHistoryDTO();
            public SocialHistoryDTO SocialHistory { get; set; } = new SocialHistoryDTO();
            public OralHealthConditionDTO OralHealthCondition { get; set; } = new OralHealthConditionDTO();
            public PresenceDTO Presence { get; set; } = new PresenceDTO();
            public ToothCountDTO ToothCount { get; set; } = new ToothCountDTO();
            public virtual ICollection<ServicesDTO>? Services { get; set; }
            public virtual ICollection<FindingsDTO>? Findings { get; set; }
            public Guid? PatientDepartmentTransactionReferenceId { get; set; }
        }

        public class MedicalHistoryDTO() {
            public Guid DentalRecordDetailsMedicalHistoryId { get; set; }
            public Boolean HasAlergies { get; set; } = false;
            [MaxLength(500)]
            public String Alergies { get; set; } = string.Empty;
            public Boolean HasHypertentionOrCVA { get; set; } = false;
            public Boolean HasDiabetesMelitus { get; set; } = false;
            public Boolean HasBloodDisorders { get; set; } = false;
            public Boolean HasCardiovascularOrHeartDiseases { get; set; } = false;
            public Boolean HasThyroidDisorders { get; set; } = false;
            public Boolean HasHepatitis { get; set; } = false;
            [MaxLength(500)]
            public String HepatitisType { get; set; } = string.Empty;
            public Boolean HasMalignancy { get; set; } = false;
            [MaxLength(500)]
            public String MalignancyType { get; set; } = string.Empty;
            public Boolean HasHistoryOfPrevHospitalization { get; set; } = false;
            [MaxLength(500)]
            public String Medical { get; set; } = string.Empty;
            [MaxLength(500)]
            public String Surgical { get; set; } = string.Empty;
            public Boolean HasBloodTransfusion { get; set; } = false;
            public int? BloodTransfusionMonth { get; set; }
            public int? BloodTransfusionYear { get; set; }
            public Boolean HasTattoo { get; set; } = false;
            public Boolean HasOthers { get; set; } = false;
            [MaxLength(500)]
            public String Others { get; set; } = string.Empty;
        }

        public class SocialHistoryDTO {
            public Guid DentalRecordDetailsSocialHistoryId { get; set; }
            public Boolean HasSweetenedSugarBeverageOrFood { get; set; } = false;
            public String SweetenedSugarBeverageOrFood { get; set; } = string.Empty;
            public Boolean HasUseOfAlcohol { get; set; } = false;
            [MaxLength(500)]
            public String UseOfAlcohol { get; set; } = string.Empty;
            public Boolean HasUseOfTobacco { get; set; } = false;
            [MaxLength(500)]
            public String UseOfTobacco { get; set; } = string.Empty;
            public Boolean HasBetelNutChewing { get; set; } = false;
            [MaxLength(500)]
            public String BetelNutChewing { get; set; } = string.Empty;
        }

        public class OralHealthConditionDTO {

            public Guid DentalRecordDetailsOralHealthConditionId { get; set; }
            public DateTime? DateOfOralExamination { get; set; }
            public Boolean OrallyFitChild { get; set; }
            public Boolean DentalCarries { get; set; }
            public Boolean Gingivitis { get; set; }
            public Boolean PeriodontalDisease { get; set; }
            public Boolean Debris { get; set; }
            public Boolean Calculus { get; set; }
            public Boolean AbnormalGrowth { get; set; }
            public Boolean CleftLipOrPalate { get; set; }
            public String Others { get; set; }
            public int NoPermTeethPresent { get; set; }
            public int NoPermSoundTeeth { get; set; }
            public int NoOfDecayedTeethBigD { get; set; }
            public int NoOfMissingTeethM { get; set; }
            public int NoOfFilledTeethBigF { get; set; }
            public int TotalDMFTeeth { get; set; }
            public int NoTempTeethPresent { get; set; }
            public int NoTempSoundTeeth { get; set; }
            public int NoOfDecayedTeethSmallD { get; set; }
            public int NoOfFilledTeethSmallF { get; set; }
            public int TotalDFTeeth { get; set; }
        }

        public class PresenceDTO {
            public Guid DentalRecordDetailsPresenceId { get; set; }
            public DateTime? DateOfExamination { get; set; }
            public int AgeLastBirthday { get; set; }
            public Boolean PresenceOfDentalCarries { get; set; } = false;
            public Boolean PresenceOfGingivitis { get; set; } = false;
            public Boolean PresenceOfPeriodicPocket { get; set; } = false;
            public Boolean PresenceOfOralDebris { get; set; } = false;
            public Boolean PresenceOfCalculus { get; set; } = false;
            public Boolean PresenceOfNeoplasm { get; set; } = false;
            public Boolean PresenceOfDentoFacialAnomaly { get; set; } = false;
        }

        public class ToothCountDTO {
            public Guid DentalRecordDetailsToothCountId { get; set; }
            public int NoOfTeethPresentTemp { get; set; }
            public int NoOfTeethPresentPerm { get; set; }
            public int CarriesIndicatedForFillingTemp { get; set; }
            public int CarriesIndicatedForFillingPerm { get; set; }
            public int CarriesIndicatedForExtractionTemp { get; set; }
            public int CarriesIndicatedForExtractionPerm { get; set; }
            public int RootFragmentTemp { get; set; }
            public int RootFragmentPerm { get; set; }
            public int MissingDueToCarries { get; set; }
            public int FilledOrRestoredTemp { get; set; }
            public int FilledOrRestoredPerm { get; set; }
            public int TotalDfAndDmfTeeth { get; set; }
            [MaxLength(500)]
            public string FluorideApplication { get; set; } = string.Empty;
            [MaxLength(500)]
            public string Examiner { get; set; } = string.Empty;

        }

        public class ServicesDTO {
            public Guid DentalRecordDetailsServicesId { get; set; }
            public Guid DentalRecordId { get; set; }
            public DateTime DateDiagnose { get; set; }
            [MaxLength(500)]
            public string Diagnosis { get; set; } = string.Empty;
            public int ToothNo { get; set; }
            [MaxLength(500)]
            public string ServiceRendered { get; set; } = string.Empty;
            [MaxLength(500)]
            public string Remarks { get; set; } = string.Empty;
        }

        public class FindingsDTO {
            public Guid DentalRecordDetailsFindingsId { get; set; }
            public Guid DentalRecordId { get; set; }
            public DateTime DateDiagnose { get; set; }
            public int ToothNo { get; set; }
            [MaxLength(300)]
            public string Condition { get; set; } = string.Empty;
            [MaxLength(300)]
            public string Diagnosis { get; set; } = string.Empty;
            [MaxLength(500)]
            public string Remarks { get; set; } = string.Empty;
        }

    }
}
