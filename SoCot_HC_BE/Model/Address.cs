﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class Address
    {

        [Key]
        public Guid AddressId { get; set; }
        public string? TempId { get; set; }
        public int BarangayId { get; set; }
        public int MunicipalityId { get; set; }
        public int ProvinceId { get; set; }
        [StringLength(50)]
        public string? Sitio { get; set; }
        [StringLength(50)]
        public string? Purok { get; set; }
        [StringLength(10)]
        public string? ZipCode { get; set; }
        [StringLength(10)]
        public string? HouseNo { get; set; }
        [StringLength(10)]
        public string? LotNo { get; set; }
        [StringLength(10)]
        public string? BlockNo { get; set; }
        [StringLength(50)]
        public string? Street { get; set; }
        [StringLength(50)]
        public string? Subdivision { get; set; }

        [ForeignKey(nameof(ProvinceId))]
        [InverseProperty(nameof(Province.Addresses))]
        public virtual Province? Province { get; set; }

        [ForeignKey(nameof(MunicipalityId))]
        [InverseProperty(nameof(Municipality.Addresses))]
        public virtual Municipality? Municipality { get; set; }

        [ForeignKey(nameof(BarangayId))]
        [InverseProperty(nameof(Barangay.Addresses))]
        public virtual Barangay? Barangay { get; set; }

        [InverseProperty(nameof(Person.AddressAsResidential))]
        public ICollection<Person> PersonsWithResidentialAddress { get; set; } = new List<Person>();
        [InverseProperty(nameof(Person.AddressAsPermanent))]
        public ICollection<Person> PersonsWithPermanentAddress { get; set; } = new List<Person>();

        [InverseProperty(nameof(Household.Address))]
        public ICollection<Household> Households { get; set; } = new List<Household>();



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
