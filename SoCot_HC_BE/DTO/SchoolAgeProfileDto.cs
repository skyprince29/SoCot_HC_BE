using SoCot_HC_BE.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.DTO
{
    public class SchoolAgeProfileDto : AuditInfoDto
    {
        public Guid SchoolAgeProfileId { get; set; }
        public Guid PersonId { get; set; }
        public bool IsInSchool { get; set; }
        public string EducationalLevel { get; set; }
        public string Grade { get; set; }
        public string SchoolYear { get; set; }
    }
}
