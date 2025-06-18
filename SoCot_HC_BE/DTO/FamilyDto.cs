namespace SoCot_HC_BE.DTO
{
    public class FamilyDto
    {
        public Guid Id { get; set; }
        public string? FamilySerialNo { get; set; }
        public Guid HouseHoldId { get; set; }
        public HouseholdDTO? HouseHold { get; set; }
        public Guid PersonHeadId { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public bool isActive { get; set; }
    }
}
