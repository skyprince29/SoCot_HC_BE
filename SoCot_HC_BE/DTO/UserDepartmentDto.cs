using SoCot_HC_BE.Model;
using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.DTO
{
    public class UserDepartmentDto
    {
        public Guid UserDepartmentId { get; set; }
        public Guid? PersonId { get; set; }
        public string Firstname { get; set; } = string.Empty;
        public string Middlename { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
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
        public Guid? DepartmentId { get; set; }
        public string DepartmentCode { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
