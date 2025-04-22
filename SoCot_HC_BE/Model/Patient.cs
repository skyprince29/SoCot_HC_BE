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

        [ForeignKey(nameof(PersonIdPatient))]
        [InverseProperty(nameof(Person.PatientsAsSelf))]
        public virtual required Person PersonAsSelf { get; set; }

        [ForeignKey(nameof(PersonIdSpouse))]
        [InverseProperty(nameof(Person.PatientsAsSpouse))]
        public virtual Person? PersonAsSpouse { get; set; }

        [ForeignKey(nameof(PersonIdMother))]
        [InverseProperty(nameof(Person.PatientsAsMother))]
        public virtual Person? PersonAsMother { get; set; }

        [ForeignKey(nameof(PersonIdFather))]
        [InverseProperty(nameof(Person.PatientsAsFather))]
        public virtual Person? PersonAsFather { get; set; }

    }
}
