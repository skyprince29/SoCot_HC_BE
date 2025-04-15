namespace SoCot_HC_BE.Model
{
    public class Barangay
    {
        public Guid BarangayId { get; set; }
        public required string BarangayName { get; set; }
        public int MunicipalityId { get; set; }
        public virtual Municipality Municipality { get; set; }
    }
}
