namespace SoCot_HC_BE.DTO.ParamDto
{
    public abstract class PageRequestParam
    {
        // Paging
        public int PageNo { get; set; }
        public int Limit { get; set; }
    }
}
