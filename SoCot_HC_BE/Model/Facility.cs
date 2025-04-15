using SoCot_HC_BE.Model.BaseModels;
using SoCot_HC_BE.Model.Enums;
using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model
{
    public class Facility : AuditInfo
    {
        [Key]
        public int FacilityId { get; set; }
        [MaxLength(50)]
        public required string FacilityCode { get; set; }
        public required Guid AddressId { get; set; }
        [MaxLength(30)]
        public string? AccreditationNo { get; set; }
        [MaxLength(300)]
        public required string FacilityName { get; set; }
        [MaxLength(30)]
        [DataType(DataType.EmailAddress)]
        public string? EmailAddress { get; set; }
        [MaxLength(15)]
        public string? TINNumber { get; set; }
        [RegularExpression("(^0[1-9][0-9]*)", ErrorMessage = "Incorrect mobile number format. (e.g. 09XX XXXX XXX)")]
        [Range(9000000000, 9999999999, ErrorMessage = "Incorrect mobile number format. (e.g. 09XX XXXX XXX)")]
        [MaxLength(15)]
        public string? ContactNumber { get; set; }
        [MaxLength(20)]
        [EnumDataType(typeof(Sector))]
        public required string Sector { get; set; }
        [MaxLength(10)]
        [EnumDataType(typeof(FacilityLevel))]
        public required string FacilityLevel { get; set; }
        public bool IsActive { get; set; }
    }
}
