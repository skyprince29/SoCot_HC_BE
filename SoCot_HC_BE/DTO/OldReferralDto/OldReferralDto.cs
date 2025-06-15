using System;

namespace SoCot_HC_BE.DTO.OldReferralDto
{
    public class ExternalApiSettings
    {
        public string? BaseUrl { get; set; }
        public string? AuthEndpoint { get; set; }
        public string? ReferralEndpoint { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class UHCReferralDTO
    {
        public ReferralDto? Referral { get; set; }
        public HouseHoldDto? HouseHold { get; set; }
        public FamilyDto? Family { get; set; }
        public FamilyMemberDto? FamilyMember { get; set; }
        public PatientDto? Patient { get; set; }
    }

    public class ReferralDto
    {
        public int Id { get; set; }
        public string? ReferralNo { get; set; }
        public TimeSpan TimeofReferral { get; set; }
        public string? Complains { get; set; }
        public string? Diagnosis { get; set; }
        public string? Reason { get; set; }
        public string? Remarks { get; set; }
        public string? BloodPressure { get; set; }
        public string? HeartRate { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public string? Temperature { get; set; }
        public string? RespirationRate { get; set; }
        public string? Status { get; set; }
        public string? ReferringFacility { get; set; }
        public int? PatientId { get; set; }
        public PatientDto? Patient { get; set; }
        public int? FacilityId { get; set; }
        public int? Referredfrom { get; set; }
        public int? PersonnelId { get; set; }
        public int? UserId { get; set; }
        public bool IsAccepted { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime? DateGenerated { get; set; }
        public bool IsUrgent { get; set; }
        public HouseHoldDto? HouseHold { get; set; }
        public FamilyDto? Family { get; set; }
        public FamilyMemberDto? FamilyMember { get; set; }
        public PersonDto? Person { get; set; }
        public UserDto? User { get; set; }
    }

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

    public class HouseHoldDto
    {
        public int Id { get; set; }
        public string? PHouseholdNo { get; set; }
        public string? ResidenceName { get; set; }
        public string? HeadofFamily { get; set; }
        public string? Address { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public bool IsActive { get; set; }
    }

    public class FamilyDto
    {
        public Guid? Id { get; set; }
        public string? FamilySerialNo { get; set; }
        public Guid HouseHoldId { get; set; }
        public HouseHoldDto? HouseHold { get; set; }
        public Guid PersonHeadId { get; set; }
        public bool IsActive { get; set; }
    }

    public class FamilyMemberDto
    {
        public int Id { get; set; }
        public int? FamilyId { get; set; }
        public FamilyDto? Family { get; set; }
        public int? PatientId { get; set; }
        public PatientDto? Patient { get; set; }
        public bool? IsActive { get; set; }
    }

    public class PersonnelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Designation {  get; set; }
        public int FacilityId {  get; set; }
        public string LicenseNo { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Suffix { get; set; }
        public string? Designation { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? ContactNumber { get; set; }
        public string? Status { get; set; }
        public DateTime? LastLogin { get; set; }
        public int? FacilityId { get; set; }
        public int? RegionID { get; set; }
        public int? ProvinceID { get; set; }
        public int? CityMunicipalityID { get; set; }
        public int? barangay_id { get; set; }
        public int DepartmentId { get; set; }
        public bool isActive { get; set; } = true;
        public string? UserIDNo { get; set; }
        public string? AccessibleModules { get; set; }
    }
}