using SoCot_HC_BE.Model;

namespace SoCot_HC_BE.DTO
{
    public class PersonDto
    {
        public Guid PersonId { get; set; }
        public string? Firstname { get; set; }
        public string? Middlename { get; set; }
        public string? Lastname { get; set; }
        public string? Suffix { get; set; }
        public DateTime BirthDate { get; set; }
        public string? BirthPlace { get; set; }
        public string? Gender { get; set; }
        public string? HouseholdNo { get; set; }
        public Guid? HouseholdId { get; set; }
        public string? FamilyNo { get; set; }
        public string? ContactNo { get; set; }
        public string? CivilStatus { get; set; }
        public string? Religion { get; set; }
        public string? Email { get; set; }
        public bool IsDeceased { get; set; }
        public string? Citizenship { get; set; }
        public string? BloodType { get; set; }
        public int? PatientIdTemp { get; set; } = null;

        public string? Fullname { get; set; }
        //public string Fullname
        //{
        //    get
        //    {
        //        var middle = !string.IsNullOrEmpty(Middlename) ? $"{Middlename[0]}." : "";
        //        return $"{Lastname}, {Firstname} {middle}".Trim();
        //    }
        //}

        public AddressDto? ResidentialAddress { get; set; }
        public AddressDto? PermanentAddress { get; set; }

        public List<FamilyMemberDTO>? familyMemberDTO { get; set; }
    }

}
