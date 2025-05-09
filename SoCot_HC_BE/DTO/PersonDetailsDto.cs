namespace SoCot_HC_BE.DTO
{
    public class PersonDetailsDto
    {
        public string FullName { get; set; } = string.Empty;
        public string? Gender { get; set; }
        public DateTime? Birthdate { get; set; }
        public int Age { get; set; }
        public string? ContactNumber { get; set; }
        public string? FullAddress { get; set; }
    }
}
