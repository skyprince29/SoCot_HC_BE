using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model
{
    public class Province
    {
        [Key]
        public int ProvinceId { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(5)]
        public string ProvinceCode { get; set; }
    }
}
