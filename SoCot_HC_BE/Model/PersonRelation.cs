using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class PersonRelation
    {
        [Key]
        public Guid PersonRelationId { get; set; }

        public Guid PersonIdSelf { get; set; }

        public Guid PersonIdRelated { get; set; }

        [MaxLength(20)]
        public required string RelationType { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey(nameof(PersonIdSelf))]
        [InverseProperty(nameof(Person.PersonRelationsAsSelf))]
        public virtual required Person PersonAsSelf { get; set; }

        [ForeignKey(nameof(PersonIdRelated))]
        [InverseProperty(nameof(Person.PersonRelationsAsRelated))]
        public virtual Person? PersonAsRelated { get; set; }
    }
}
