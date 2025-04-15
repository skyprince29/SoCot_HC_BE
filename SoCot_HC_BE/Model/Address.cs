using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model
{
    public class Address
    {



        [Key]
        public Guid AddressId { get; set; }
        public int BarangayId { get; set; }
        public int MunicipalityId { get; set; }
        public int ProvinceId { get; set; }
        [StringLength(30)]
        public string? Sitio { get; set; }
        [StringLength(30)]
        public string? Purok { get; set; }
        [StringLength(10)]
        public string? ZipCode { get; set; }
        [StringLength(10)]
        public string? HouseNo { get; set; }
        [StringLength(10)]
        public string? LotNo { get; set; }
        [StringLength(10)]
        public string? BlockNo { get; set; }
        [StringLength(30)]
        public string? Street { get; set; }
        [StringLength(30)]
        public string? Subdivision { get; set; }

        [ForeignKey(nameof(BarangayId))]
        [InverseProperty("Address")]
        public virtual required Barangay Barangay { get; set; }

        [ForeignKey(nameof(MunicipalityId))]
        [InverseProperty("Address")]
        public virtual required Municipality Municipality { get; set; }

        [ForeignKey(nameof(ProvinceId))]
        [InverseProperty("Address")]
        public virtual required Province Province { get; set; }

        [NotMapped]
        public string FullAddress
        {
            get
            {
                var parts = new List<string>();

                if (!string.IsNullOrWhiteSpace(HouseNo)) parts.Add($"House No. {HouseNo.Trim()}");
                if (!string.IsNullOrWhiteSpace(LotNo)) parts.Add($"Lot {LotNo.Trim()}");
                if (!string.IsNullOrWhiteSpace(BlockNo)) parts.Add($"Block {BlockNo.Trim()}");
                if (!string.IsNullOrWhiteSpace(Street)) parts.Add(Street.Trim());
                if (!string.IsNullOrWhiteSpace(Sitio)) parts.Add($"Sitio {Sitio.Trim()}");
                if (!string.IsNullOrWhiteSpace(Purok)) parts.Add($"Purok {Purok.Trim()}");
                if (!string.IsNullOrWhiteSpace(Subdivision)) parts.Add(Subdivision.Trim());

                if (Barangay != null && !string.IsNullOrWhiteSpace(Barangay.BarangayName))
                    parts.Add($"Brgy. {Barangay.BarangayName.Trim()}");

                if (Municipality != null && !string.IsNullOrWhiteSpace(Municipality.MunicipalityName))
                    parts.Add(Municipality.MunicipalityName.Trim());

                if (Province != null && !string.IsNullOrWhiteSpace(Province.ProvinceName))
                    parts.Add(Province.ProvinceName.Trim());

                if (!string.IsNullOrWhiteSpace(ZipCode)) parts.Add(ZipCode.Trim());

                return string.Join(", ", parts);
            }
        }
    }
}
