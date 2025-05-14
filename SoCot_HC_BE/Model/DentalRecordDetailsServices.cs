using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model
{
    public class DentalRecordDetailsServices
    {
        public Guid DentalRecordDetailsServicesId { get; set; }
        public Guid DentalRecordId { get; set; }
        public DateTime DateDiagnose { get; set; }
        [MaxLength(500)]
        public string Diagnosis { get; set; } = string.Empty;
        public int ToothNo { get; set; }
        [MaxLength(500)]
        public string ServiceRendered { get; set; } = string.Empty;
        [MaxLength(500)]
        public string Remarks { get; set; } = string.Empty;
    }
}
