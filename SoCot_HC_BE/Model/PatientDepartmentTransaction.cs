using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SoCot_HC_BE.Model.BaseModels;
using SoCot_HC_BE.Model.Enums;

namespace SoCot_HC_BE.Model
{
    public class PatientDepartmentTransaction : BaseTransaction
    {
        public PatientDepartmentTransaction()
        {
            base.ModuleId = (int)ModuleEnum.PatientDepartmentTransaction;
        }

        [NotMapped]
        public override Guid TransactionId => PatientRegistryId;

        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid PatientRegistryId { get; set; }
        [ForeignKey("PatientRegistryId")]
        public virtual PatientRegistry? PatientRegistry { get; set; }

        public Guid? FromDepartmentId { get; set; }

        public Guid? DepartmentId { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        public Guid? ForwardedBy { get; set; } // id of user

        public Guid? AcceptedBy { get; set; } // id of user

        [ForeignKey("StatusId")]
        public virtual Status? Status { get; set; }
        public bool IsCompleted { get; set; }

        public bool IsActive { get; set; } = true;

        public string? Remarks { get; set; }
    }
}
