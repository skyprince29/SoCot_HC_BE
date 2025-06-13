using SoCot_HC_BE.Model.BaseModels;
using SoCot_HC_BE.Model.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class SchoolAgeProfile : AuditInfo
    {
        [Key]
        public Guid SchoolAgeProfileId { get; set; }

        [ForeignKey("Person")]
        public Guid PersonId { get; set; }
        public Person? Person { get; set; }

        [Required]
        public bool IsInSchool { get; set; }

        public string EducationalLevel { get; set; } = string.Empty;

        public string Grade { get; set; } = string.Empty;

        public string SchoolYear { get; set; } = string.Empty;
    }
}
