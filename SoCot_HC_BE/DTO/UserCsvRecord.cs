namespace SoCot_HC_BE.DTO
{
    public class UserCsvRecord
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Field1 { get; set; }
        public string? Field2 { get; set; }
        public int SomeId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string ContactNo { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string? Field3 { get; set; }
        public string? Field4 { get; set; }
        public int FacilityId { get; set; }
        public int ProvinceId { get; set; }
        public int MunicipalityId { get; set; }
        public int BarangayId { get; set; }
        public string? Field5 { get; set; }
        public int SomeFlag { get; set; }
        public string? Field6 { get; set; }
        public int AnotherField { get; set; }
    }
}
