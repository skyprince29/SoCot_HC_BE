using SoCot_HC_BE.Model.BaseModels;
using SoCot_HC_BE.Model.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class WRA : AuditInfo
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Person")]
        public Guid PersonId { get; set; }
        public Person? Person { get; set; }

        //WRA PROFILE
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? WraDateOfAssessment { get; set; }
        public string Fecundity { get; set; } = String.Empty;
        public bool HavePartner { get; set; }
        public bool WraPlanToHaveMoreChildren { get; set; }
        public string WraPlanToHveMoreChildrenDecision { get; set; } = String.Empty;
        public bool ForCounseling { get; set; }
        public bool UsingAnyFPMethod { get; set; }
        public string FPType { get; set; } = String.Empty;
        public string FPMethod { get; set; } = String.Empty;
        public bool ShiftToModernMethod { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime WraDateRecorded { get; set; } = DateTime.Now;
    }
}
