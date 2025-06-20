using Microsoft.AspNetCore.Routing.Constraints;

namespace SoCot_HC_BE.DTO
{
    public class UploadedCSVSummaryDto
    {
        public int TotalSaved { get; set; }
        public int TotalFailed { get; set; }
        public int TotalRecords { get; set; }
    }
}
