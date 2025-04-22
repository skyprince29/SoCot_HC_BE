using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
        [MaxLength(5)]
        public string? Suffix { get; set; }
        public DateTime BirthDate { get; set; }
        [MaxLength(50)]
        public string? BirthPlace { get; set; }
        [MaxLength(10)]
        public required string Gender { get; set; }
        [MaxLength(20)]
        public string? CivilStatus { get; set; }
        [MaxLength(30)]
        public string? Religion { get; set; }
        [MaxLength(15)]
        public string? ContactNo { get; set; }
        [MaxLength(50)]
        public string? Email { get; set; }
        public Guid? AddressIdResidential { get; set; }
        public Guid? AddressIdPermanent { get; set; }
        public bool IsDeceased { get; set; }
        [MaxLength(30)]
        public string? Citizenship { get; set; }
        [MaxLength(3)]
        public string? BloodType { get; set; }


        [ForeignKey(nameof(AddressIdResidential))]
        [InverseProperty("PersonsWithResidentialAddress")]
        public virtual Address? ResidentialAddress { get; set; }

        [ForeignKey(nameof(AddressIdPermanent))]
        [InverseProperty("PersonsWithPermanentAddress")]
        public virtual Address? PermanentAddress { get; set; }


        [InverseProperty("HoHPerson")]
        public virtual ICollection<Household> HouseholdWithHoHPerson { get; set; } = new List<Household>();


        [InverseProperty("PatientPerson")]
        public ICollection<Patient> PatientsAsSelf { get; set; } = new List<Patient>();

        [InverseProperty("SpousePerson")]
        public ICollection<Patient> PatientsAsSpouse { get; set; } = new List<Patient>();

        [InverseProperty("MotherPerson")]
        public ICollection<Patient> PatientsAsMother { get; set; } = new List<Patient>();

        [InverseProperty("FatherPerson")]
        public ICollection<Patient> PatientsAsFather { get; set; } = new List<Patient>();





    }
}
