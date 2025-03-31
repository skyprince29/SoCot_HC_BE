using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class VitalSign
    {
        [Key]
        public long VitalSignId { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal Temperature { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal Height { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal Weight { get; set; }
        public int RespiratoryRate { get; set; }
        public int CardiacRate { get; set; }
        public int Systolic {  get; set; }
        public int Diastolic {  get; set; }
    }
}
