using SoCot_HC_BE.Model.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class VitalSignReference
    {
        [Key]
        public Guid VitalSignReferenceId { get; set; }
        [ForeignKey("VitalSignId")]
        public Guid VitalSignId { get; set; }
        public Guid ReferenceId { get; set; } // ID of the related entity
        public VitalSignReferenceType VitalSignReferenceType { get; set; }
    }
}
