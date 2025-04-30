using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoCot_HC_BE.Model
{
    public class UserGroup
    {
        [Key]
        public int UserGroupId { get; set; }
  
        public int UserType { get; set; }

        [MaxLength(50)]
        public required string UserGroupName { get; set; }

        public bool IsActive { get; set; }

        [InverseProperty(nameof(UserAccount.UserGroupAsUserAccount))]
        public ICollection<UserAccount> UserAccountsAsUserGroup { get; set; } = new List<UserAccount>();
    }
}
