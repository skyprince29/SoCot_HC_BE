using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model
{
    public class DentalRecordDetailsOralHealthCondition
    {
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
        [MaxLength(500)]
        public String Others { get; set; } = string.Empty;
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
}
