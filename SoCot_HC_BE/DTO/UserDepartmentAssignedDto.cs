namespace SoCot_HC_BE.DTO
{
    public class UserDepartmentAssignedDto
    {
        public Guid? UserDepartmentId { get; set; }
        public Guid? PersonId { get; set; }
        public Guid? userAccountId { get; set; }
        public string Firstname { get; set; } = string.Empty;
        public string Middlename { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Fullname
        {
            get
            {
                // Ensure names are not null before trimming
                string firstNamePart = Firstname?.Trim() ?? string.Empty;
                string middleNamePart = Middlename?.Trim() ?? string.Empty;
                string lastNamePart = Lastname?.Trim() ?? string.Empty;

                if (!string.IsNullOrEmpty(middleNamePart))
                {
                    return $"{lastNamePart}, {firstNamePart} {middleNamePart}";
                }
                else
                {
                    return $"{lastNamePart}, {firstNamePart}";
                }
            }
        }

        public int TotalAssignedDepartments { get; set; }
    }
}
