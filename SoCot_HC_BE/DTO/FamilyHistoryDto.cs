namespace SoCot_HC_BE.DTO
{
    public class FamilyHistoryDto
    {
        public Guid Id { get; set; }
        public bool Hypertension { get; set; } = false;
        public bool Stroke { get; set; }
        public bool HeartAttack { get; set; }
        public bool Diabetes { get; set; }
        public bool Asthma { get; set; }
        public bool Cancer { get; set; }
        public bool KidneyDisease { get; set; }
    }
}
