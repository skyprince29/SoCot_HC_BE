using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class Patient
    {

        [Key]
        public Guid PatientId { get; set; }
        [MaxLength(30)]
        public required string PHICMemberType { get; set; }
        [MaxLength(50)]
        public required string PhilHealthNo { get; set; }

        public Guid PersonIdPatient { get; set; }

        public Guid? PersonIdSpouse { get; set; }
        public Guid? PersonIdMother { get; set; }
        public Guid? PersonIdFather { get; set; }
        public int PatientIdTemp { get; set; }
        public bool IsActive { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(PersonIdPatient))]
        [InverseProperty("PatientsAsSelf")]
        public virtual required Person PatientPerson { get; set; }

        [ForeignKey(nameof(PersonIdSpouse))]
        [InverseProperty("PatientsAsSpouse")]
        public virtual Person? SpousePerson { get; set; }

        [ForeignKey(nameof(PersonIdMother))]
        [InverseProperty("PatientsAsMother")]
        public virtual Person? MotherPerson { get; set; }

        [ForeignKey(nameof(PersonIdFather))]
        [InverseProperty("PatientsAsFather")]
        public virtual Person? FatherPerson { get; set; }


    }
}
