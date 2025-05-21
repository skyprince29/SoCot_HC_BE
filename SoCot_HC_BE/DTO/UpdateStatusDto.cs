namespace SoCot_HC_BE.DTO
{
    public class UpdateStatusDto
    {
        public Guid TransactionId { get; set; }
        public int ModuleId { get; set; }
        public byte? StatusId { get; set; }
        public string? Remarks { get; set; }
    }
}
