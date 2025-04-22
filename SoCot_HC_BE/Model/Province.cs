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

        [InverseProperty(nameof(Address.Province))]
        public ICollection<Address> Addresses { get; set; } = new List<Address>();

        [InverseProperty(nameof(Municipality.Province))]
        public ICollection<Municipality> Municipalities { get; set; } = new List<Municipality>();

    }
}
