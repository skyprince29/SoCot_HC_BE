namespace SoCot_HC_BE.DTO
{
    public class PatientForwardingDto
    {
        public Guid PatientRegistryId { get; set; }
        public Guid? CurrentPatientDepartmentTransactionId { get; set; }
        public Guid? FromDepartmentId { get; set; }
        public Guid CurrentDepartmentId { get; set; }
        public Guid ForwardedByUserId { get; set; }
        public string? Remarks { get; set; }
        public bool IsTransfer { get; set; }
    }
}
