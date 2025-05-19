using SoCot_HC_BE.Model.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class UserAccount : AuditInfo
    {
        [Key]
        public Guid UserAccountId { get; set; }

        [MaxLength(50)]
        public required string Username { get; set; }

        [MaxLength(100)]
        public required string Password { get; set; }

        public Guid PersonId { get; set; }

        public int FacilityId { get; set; }

        public int UserGroupId { get; set; }

        [MaxLength(100)]
        public string? RememberMeToken { get; set; }
        public bool IsOnline { get; set; }
        public bool IsinitLogin { get; set; }
        public bool IsActive { get; set; }
        public int? UserIdTemp { get; set; }

        // --- Navigation Properties ---
        [ForeignKey(nameof(PersonId))]
        [InverseProperty(nameof(Person.UserAccountsAsPerson))]
        public virtual Person? PersonAsUserAccount { get; set; }

        [ForeignKey(nameof(FacilityId))]
        [InverseProperty(nameof(Facility.UserAccountsAsFacility))]
        public virtual Facility? FacilityAsUserAccount { get; set; }

        [ForeignKey(nameof(UserGroupId))]
        [InverseProperty(nameof(UserGroup.UserAccountsAsUserGroup))]
        public virtual UserGroup? UserGroupAsUserAccount { get; set; }
    }
}
