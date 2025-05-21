using SoCot_HC_BE.Model;

namespace SoCot_HC_BE.DTO
{
    public class UserAccountTokenDTO
    {
        public UserAccount? userAccount { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
