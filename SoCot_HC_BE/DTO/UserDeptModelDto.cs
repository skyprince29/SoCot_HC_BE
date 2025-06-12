namespace SoCot_HC_BE.DTO
{
    public class UserDeptModelDto
    {
        public Guid personId { get; set; }
        public Guid userAccountId { get; set; }
        public required List<Guid> departmentIds { get; set; }
    }
}
