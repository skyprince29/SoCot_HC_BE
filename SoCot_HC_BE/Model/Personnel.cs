using SoCot_HC_BE.Model.BaseModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class Personnel : AuditInfo
    {
        [Key]
        public Guid PersonnelId { get; set; }
        public required int FacilityId { get; set; }

        [ForeignKey("FacilityId")]
        public Facility? Facility { get; set; }
        public required Guid PersonId { get; set; }

        [ForeignKey("PersonId")]
        public Person? Person { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("DesignationId")]
        public Designation? Designation { get; set; }
        public Guid DesignationId { get; set; } 
    }
}
