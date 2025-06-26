namespace SoCot_HC_BE.DTO.ParamDto
{
    public class GetPagedPDTRequestParam : PageRequestParam
    {
        // Required
        public Guid CurrentDepartmentId { get; set; }
        public Boolean isForAccept { get; set; }
        public Boolean isForForward { get; set; }

        // Optional
        public Guid? FromDepartmentId { get; set; }
        public string? Keyword { get; set; }
        public byte? StatusId { get; set; }
        public Guid? UserId { get; set; }

        // Required: Date range
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
