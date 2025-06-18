namespace SoCot_HC_BE.DTO
{
    public class PatientDto
    {
        public int Id { get; set; }
        public string? PHouseholdNo { get; set; }
        public string? FamilySerialNo { get; set; }
        public string? PhilHealthNo { get; set; }
        public string? Lastname { get; set; }
        public string? Firstname { get; set; }
        public string? Middlename { get; set; }
        public string? Sex { get; set; }
        public string? CivilStatus { get; set; }
        public DateTime? Birthdate { get; set; }
        public int? Age { get; set; }
        public string? BloodType { get; set; }
        public string? BirthPlace { get; set; }
        public string? EmploymentStatus { get; set; }
        public string? EducationalAttainment { get; set; }
        public string? Occupation { get; set; }
        public bool? IsIndigent { get; set; }
        public string? ContactNumber { get; set; }
        public string? Citizenship { get; set; }
        public string? SpouseLname { get; set; }
        public string? SpouseFname { get; set; }
        public string? SpouseMname { get; set; }
        public string? FatherLname { get; set; }
        public string? FatherFname { get; set; }
        public string? MotherFname { get; set; }
        public bool? IsActive { get; set; }
    }
}
