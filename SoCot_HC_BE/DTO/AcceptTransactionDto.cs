namespace SoCot_HC_BE.DTO
{
    public class AcceptTransactionDto
    {
        public Guid Id { get; set; }             // The transaction ID
        public Guid AcceptedBy { get; set; }     // The user who accepted it
    }
}
