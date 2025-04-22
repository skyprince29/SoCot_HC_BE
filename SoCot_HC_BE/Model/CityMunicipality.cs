using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class Municipality
    {
        [Key]
        public int MunicipalityId {  get; set; }
        public int ProvinceId { get; set; }
        [MaxLength(100)]
        public required string MunicipalityName { get; set; }

        [ForeignKey(nameof(ProvinceId))]
        [InverseProperty(nameof(Province.Municipalities))]
        public virtual required Province Province { get; set; }

        [InverseProperty(nameof(Barangay.Municipality))]
        public ICollection<Barangay> Barangays { get; set; } = new List<Barangay>();

        [InverseProperty(nameof(Address.Municipality))]
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}
