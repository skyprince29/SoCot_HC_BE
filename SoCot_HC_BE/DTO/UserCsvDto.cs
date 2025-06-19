namespace SoCot_HC_BE.DTO
{
    public class UserCsvDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? LastLogin { get; set; }
        public int FacilityId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Suffix { get; set; } = string.Empty;
        public int RegionID { get; set; }
        public int ProvinceID { get; set; }
        public int CityMunicipalityID { get; set; }
        public int BarangayId { get; set; }
        public string UserIDNo { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string AccessibleModules { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
    }
}
