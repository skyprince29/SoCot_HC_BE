namespace SoCot_HC_BE.Model
{
    public class DentalRecordDetailsPresence
    {
        public Guid DentalRecordDetailsPresenceId { get; set; }
        public DateTime? DateOfExamination { get; set; }
        public int AgeLastBirthday { get; set; }
        public Boolean PresenceOfDentalCarries { get; set; }
        public Boolean PresenceOfGingivitis { get; set; }
        public Boolean PresenceOfPeriodicPocket { get; set; }
        public Boolean PresenceOfOralDebris { get; set; }
        public Boolean PresenceOfCalculus { get; set; }
        public Boolean PresenceOfNeoplasm { get; set; }
        public Boolean PresenceOfDentoFacialAnomaly { get; set; }
    }
}
