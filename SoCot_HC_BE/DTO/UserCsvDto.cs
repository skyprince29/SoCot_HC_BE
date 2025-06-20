namespace SoCot_HC_BE.DTO
{
    public class UserCsvDto
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Designation { get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string Status { get; set; } = "";
        public string LastLogin { get; set; } = "";
        public string FacilityId { get; set; } = "";
        public string Email { get; set; } = "";
        public string ContactNumber { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = "";
        public string? Suffix { get; set; }
        public string? RegionID { get; set; }
        public string ProvinceID { get; set; } = "";
        public string CityMunicipalityID { get; set; } = "";
        public string barangay_id { get; set; } = "";
        public string? UserIDNo { get; set; }
        public bool isActive { get; set; }
        public string? AccessibleModules { get; set; }
        public string DepartmentId { get; set; }
    }
}
