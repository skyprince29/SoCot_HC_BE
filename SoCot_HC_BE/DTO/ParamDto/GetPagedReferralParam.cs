namespace SoCot_HC_BE.DTO.ParamDto
{
    public class GetPagedReferralParam : PageRequestParam
    {
        // Required
        public int ReferredFrom { get; set; }
        public int? ReferredTo { get; set; }

        // Optional
        public Guid? FromDepartmentId { get; set; }
        public string? Keyword { get; set; }
        public byte? StatusId { get; set; }

        // Required: Date range
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
