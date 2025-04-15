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
        [ForeignKey("MunicipalityId")]
        public virtual required Municipality Municipality { get; set; }

        [InverseProperty("Barangay")]
        public virtual ICollection<Address> Address { get; set; } = new List<Address>();
    }
}
