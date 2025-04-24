using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class Barangay
    {
        [Key]
        public int BarangayId { get; set; }
        public required string BarangayName { get; set; }
        public int MunicipalityId { get; set; }

        [ForeignKey(nameof(MunicipalityId))]
        [InverseProperty(nameof(Municipality.Barangays))]
        public virtual Municipality? Municipality { get; set; }

        [InverseProperty(nameof(Address.Barangay))]
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}
