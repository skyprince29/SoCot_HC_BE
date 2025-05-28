namespace SoCot_HC_BE.DTO
{
    // Adding of Member to Existing Family
    public class AppendMemberRequest
    {
        public Guid HouseholdId { get; set; }
        public Guid FamilyId { get; set; }
        public PersonSaveDto? Person { get; set; }
    }

    // Adding of Family to Existing Household
    public class AppendFamilyRequest
    {
        public Guid HouseholdId { get; set; }
        public FamilySaveDto Family { get; set; } = new FamilySaveDto(); 
    }

    public class FamilySaveDto
    {
        public Guid FamilyId { get; set; }
        public string? FamilyNo { get; set; }
        public List<PersonSaveDto> Persons { get; set; } = new List<PersonSaveDto>();
    }

    public class PersonSaveDto
    {
        public Guid PersonId { get; set; }
        public required string Firstname { get; set; }
        public string? Middlename { get; set; }
        public required string Lastname { get; set; }
        public string? Suffix { get; set; }
        public required string Birthdate { get; set; }
        public string? Birthplace { get; set; }
        public string? Gender { get; set; }
        public string? CivilStatus { get; set; }
        public string? Religion { get; set; }
        public string? ContactNo { get; set; }
        public string? Email { get; set; }
        public string? Citizenship { get; set; }
        public string? BloodType { get; set; }
    }
}
