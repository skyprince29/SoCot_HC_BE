using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class Family
    {
        [Key]
        public Guid FamilyId { get; set; }
        //public int? TempFamilyId { get; set; }
        [Required]
        [MaxLength(30)]
        public string? FamilyNo { get; set; }

        public Guid HouseholdId { get; set; }
        //public int? TempHouseholdId { get; set; }
        public Guid PersonId { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey(nameof(HouseholdId))]
        [InverseProperty(nameof(Household.Families))] // pointing to ICollection<Family> in Household
        public virtual Household? Household { get; set; }

        [ForeignKey(nameof(PersonId))]
        [InverseProperty(nameof(Person.Families))] // pointing to ICollection<Family> in Person
        public virtual Person? Person { get; set; }
    }

}
