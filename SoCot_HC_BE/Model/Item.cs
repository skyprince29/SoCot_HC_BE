using SoCot_HC_BE.Model.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class Item  :AuditInfo
    {
        [Key]
        public Guid ItemId { get; set; }
        public Guid ItemCategoryId { get; set; }
        [ForeignKey("ItemCategoryId")]
        public  required ItemCategory ItemCategory { get; set; }
        public Guid? SubCategoryId { get; set; }
        [ForeignKey("SubCategoryId")]
        public  SubCategory? SubCategory { get; set; }

        public  Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public  required Product Product { get; set; }


        [MaxLength(10)]
        public required string Code { get; set; }

        [MaxLength(50)]
        public required string Description { get; set; }
        [MaxLength(50)]
        public required string BrandName { get; set; }

        public Guid? FormId { get; set; }
        [ForeignKey("FormId")]
        public Form? Form { get; set; }

        public Guid? StrengthId { get; set; }
        [ForeignKey("StrengthId")]
        public  Strength? Strength { get; set; }

        public int? StrengthNo { get; set; }

        public Guid? RouteId { get; set; }
        [ForeignKey("RouteId")]
        public  Route? Route { get; set; }
        public Guid UoMId { get; set; }
        [ForeignKey("UoMId")]
        public required UoM UoM { get; set; }

        public bool IsActive { get; set; }
    }
}
