using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SoCot_HC_BE.Model.Enums;
using SoCot_HC_BE.Models.Enums;

namespace SoCot_HC_BE.Model
{
    public class Person
    {
        [Key]
        public Guid PersonId { get; set; }

        [MaxLength(50)]
        public required string Firstname { get; set; }

        [MaxLength(30)]
        public string? Middlename { get; set; }

        [MaxLength(30)]
        public required string Lastname { get; set; }

        public Suffix? Suffix { get; set; }

        public DateTime BirthDate { get; set; }

        [MaxLength(100)]
        public string? BirthPlace { get; set; }

        public Gender? Gender { get; set; }

        public CivilStatus? CivilStatus { get; set; }

        public Religion? Religion { get; set; }

        [MaxLength(15)]
        public string? ContactNo { get; set; }

        [MaxLength(50)]
        public string? Email { get; set; }

        public Guid? AddressIdResidential { get; set; }
        public Guid? AddressIdPermanent { get; set; }

        public bool IsDeceased { get; set; }

        [MaxLength(100)]
        public string? Citizenship { get; set; }
        public BloodType? BloodType { get; set; }

        public int PatientIdTemp { get; set; }

        // --- Navigation Properties ---
        [ForeignKey(nameof(AddressIdResidential))]
        [InverseProperty(nameof(Address.PersonsWithResidentialAddress))]
        public virtual Address? AddressAsResidential { get; set; }

        [ForeignKey(nameof(AddressIdPermanent))]
        [InverseProperty(nameof(Address.PersonsWithPermanentAddress))]
        public virtual Address? AddressAsPermanent { get; set; }

        [InverseProperty(nameof(Patient.PersonAsSelf))]
        public ICollection<Patient> PatientsAsSelf { get; set; } = new List<Patient>();

        [InverseProperty(nameof(Patient.PersonAsSpouse))]
        public ICollection<Patient> PatientsAsSpouse { get; set; } = new List<Patient>();

        [InverseProperty(nameof(Patient.PersonAsMother))]
        public ICollection<Patient> PatientsAsMother { get; set; } = new List<Patient>();

        [InverseProperty(nameof(Patient.PersonAsFather))]
        public ICollection<Patient> PatientsAsFather { get; set; } = new List<Patient>();

        [InverseProperty(nameof(Household.PersonAsHeadOfHousehold))]
        public ICollection<Household> Households { get; set; } = new List<Household>();

        [InverseProperty(nameof(Family.Person))]
        public ICollection<Family> Families { get; set; } = new List<Family>();
    }
}
