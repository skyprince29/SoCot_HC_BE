using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.DTO
{
    public class VitalSignDto : AuditInfoDto
    {
        public Guid VitalSignId { get; set; }
        public Guid? PatientRegistryId { get; set; }
        public decimal? Temperature { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public int? RespiratoryRate { get; set; }
        public int? CardiacRate { get; set; }
        public int Systolic { get; set; }
        public int Diastolic { get; set; }
        public string? BloodPressure { get; set; }
    }
}
