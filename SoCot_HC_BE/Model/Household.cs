using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class Household
    {

        [Key]
        public Guid HouseholdId { get; set; }
        [MaxLength(30)]
        public required string HouseholdNo { get; set; }
        [MaxLength(50)]
        public required string ResidenceName { get; set; }

        public Guid PersonIdHeadOfHousehold { get; set; }
        public Guid AddressId { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey(nameof(PersonIdHeadOfHousehold))]
        [InverseProperty("HouseholdWithHoHPerson")]
        public virtual required Person HoHPerson { get; set; }

        [ForeignKey(nameof(AddressId))]
        [InverseProperty("HouseholdAddress")]
        public virtual required Address HouseholdAddress { get; set; }

    }
}
