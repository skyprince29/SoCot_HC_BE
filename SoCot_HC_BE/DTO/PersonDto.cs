namespace SoCot_HC_BE.DTO
{
    public class PersonDto
    {
        public Guid PersonId { get; set; }
        public string? Firstname { get; set; }
        public string? Middlename { get; set; }
        public string? Lastname { get; set; }
        public DateTime BirthDate { get; set; }
        public string? Gender { get; set; }
        public string? HouseholdNo { get; set; }
        public string? FamilyNo { get; set; }
        public string? ContactNo { get; set; }

        public string Fullname
        {
            get
            {
                var middle = !string.IsNullOrEmpty(Middlename) ? $"{Middlename[0]}." : "";
                return $"{Lastname}, {Firstname} {middle}".Trim();
            }
        }

        public AddressDto? ResidentialAddress { get; set; }
        public AddressDto? PermanentAddress { get; set; }
    }

}
