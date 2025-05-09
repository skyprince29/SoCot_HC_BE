namespace SoCot_HC_BE.DTO
{
    public class AddressDto
    {
        public Guid AddressId { get; set; }

        public int BarangayId { get; set; }

        public int MunicipalityId { get; set; }

        public int ProvinceId { get; set; }

        public string? Sitio { get; set; }

        public string? Purok { get; set; }

        public string? ZipCode { get; set; }

        public string? HouseNo { get; set; }

        public string? LotNo { get; set; }

        public string? BlockNo { get; set; }

        public string? Street { get; set; }

        public string? Subdivision { get; set; }

        public string? FullAddress { get; set; } // Can be computed in service layer
    }
}
