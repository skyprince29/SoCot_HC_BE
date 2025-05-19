using System.ComponentModel.DataAnnotations;

namespace SoCot_HC_BE.DTO
{
    public class UserAccountDTO
    {
        public Guid? UserAccountId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public Guid PersonId { get; set; }
        public int? FacilityId { get; set; }
        public int UserGroupId { get; set; }
        public string? RememberMeToken { get; set; }
        public bool IsOnline { get; set; }
        public bool IsinitLogin { get; set; }
        public bool IsActive { get; set; }
        public int? UserIdTemp { get; set; }
    }
}
