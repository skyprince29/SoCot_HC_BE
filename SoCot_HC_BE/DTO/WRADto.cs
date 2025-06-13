using SoCot_HC_BE.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.DTO
{
    public class WRADto : AuditInfoDto
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public DateTime WraDateOfAssessment { get; set; } = DateTime.Now;
        public string Fecundity { get; set; }
        public bool HavePartner { get; set; }
        public bool WraPlanToHaveMoreChildren { get; set; }
        public string WraPlanToHveMoreChildrenDecision { get; set; }
        public bool ForCounseling { get; set; }
        public bool UsingAnyFPMethod { get; set; }
        public string FPType { get; set; }
        public string FPMethod { get; set; }
        public bool ShiftToModernMethod { get; set; }
        public DateTime WraDateRecorded { get; set; } = DateTime.Now;
    }
}
