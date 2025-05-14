using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model
{
    public class DentalRecordDetailsFindings
    {
        public Guid DentalRecordDetailsFindingsId { get; set; }
        public Guid DentalRecordId { get; set; }
        public DateTime DateDiagnose { get; set; }
        public int ToothNo { get; set; }
        [MaxLength(300)]
        public string Condition { get; set; } = string.Empty;
        [MaxLength(300)]
        public string Diagnosis { get; set; } = string.Empty;
        [MaxLength(500)]
        public string Remarks { get; set; } = string.Empty;
    }
}
