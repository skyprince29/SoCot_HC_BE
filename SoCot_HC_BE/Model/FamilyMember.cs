using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.Model
{
    public class FamilyMember
    {
        [Key]
        public Guid FamilyMemberId { get; set; }

        public Guid FamilyId { get; set; }
        public Guid PersonId { get; set; }

        [ForeignKey(nameof(FamilyId))]
        public virtual Family? Family { get; set; }

        [ForeignKey(nameof(PersonId))]
        public virtual Person? Person { get; set; }
    }
}
