using SoCot_HC_BE.Model.BaseModels;
using SoCot_HC_BE.Model.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class Facility : AuditInfo
    {
        [Key]
        public int FacilityId { get; set; }
        [MaxLength(50)]
        public required string FacilityCode { get; set; }
        public required Guid AddressId { get; set; }
        [ForeignKey("AddressId")]
        public virtual required Address Address { get; set; }
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
        public required Sector Sector { get; set; }
        public required FacilityLevel FacilityLevel { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty(nameof(UserAccount.FacilityAsUserAccount))]
        public ICollection<UserAccount> UserAccountsAsFacility { get; set; } = new List<UserAccount>();
    }
}
