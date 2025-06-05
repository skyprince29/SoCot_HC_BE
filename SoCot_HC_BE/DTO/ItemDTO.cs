using SoCot_HC_BE.Model.BaseModels;

namespace SoCot_HC_BE.DTO
{
    public class ItemDTO : AuditInfo
    {
        public Guid ItemId { get; set; }
        public Guid ItemCategoryId { get; set; }
        public Guid? SubCategoryId { get; set; }
        public Guid ProductId { get; set; }
        public  string? Code { get; set; }
        public required string Description { get; set; }
        public  string? BrandName { get; set; }
        public Guid? FormId { get; set; }
        public Guid? StrengthId { get; set; }
        public int? StrengthNo { get; set; }
        public Guid? RouteId { get; set; }
        //public Guid UoMId { get; set; }


        public bool IsActive { get; set; }
    }
}
