using SoCot_HC_BE.Model.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class Household : AuditInfo
    {

        [Key]
        public Guid HouseholdId { get; set; }
        //public int? TempHouseholdId { get; set; }
        [Required]
        [MaxLength(30)]
        public string? HouseholdNo { get; set; }
        [Required]
        [MaxLength(100)]
        public string? ResidenceName { get; set; }
        public Guid? PersonIdHeadOfHousehold { get; set; }
        public Guid AddressId { get; set; }
        public bool IsActive { get; set; }


        [ForeignKey(nameof(PersonIdHeadOfHousehold))]
        [InverseProperty(nameof(Person.Households))]
        public virtual Person? PersonAsHeadOfHousehold { get; set; }


        [ForeignKey(nameof(AddressId))]
        [InverseProperty(nameof(Address.Households))]
        public virtual Address? Address { get; set; }


        [InverseProperty(nameof(Family.Household))]
        public ICollection<Family> Families { get; set; } = new List<Family>();

    }
}
