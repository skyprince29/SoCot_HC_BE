using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class Province
    {
        [Key]
        public int ProvinceId { get; set; }
        [MaxLength(100)]
        public required string ProvinceName { get; set; }

        [InverseProperty("Province")]
        public virtual ICollection<Address> Address { get; set; } = new List<Address>();

    }
}
