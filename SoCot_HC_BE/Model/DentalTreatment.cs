using SoCot_HC_BE.Models.Enums;
using SoCot_HC_BE.Model.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;
using SoCot_HC_BE.Model.Enums;

namespace SoCot_HC_BE.Model
{

    public class DentalTreatment : AuditInfo
    {
        public Guid DentalTreatmentId { get; set; }

        [ForeignKey("Person")]
        public Guid PatientId { get; set; }
        public required Person Patient { get; set; }

        [ForeignKey("Facility")]
        public int FacilityId { get; set; }
        public required Facility Facility { get; set; }


        [ForeignKey("PatientRegistryId")]
        public virtual PatientRegistry? PatientRegistry { get; set; }

        public DateTime DatePreferred { get; set; }
        public DateTime? DateAccepted { get; set; }
        public Guid? acceptedById { get; set; }
        public int QueueNo { get; set; }

        public DentalTreatmentStatus Status { get; set; } // Changed to use the enum
    } 
}
