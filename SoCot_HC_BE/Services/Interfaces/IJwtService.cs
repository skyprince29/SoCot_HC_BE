using System.Security.Claims;

namespace SoCot_HC_BE.Services.Interfaces
{
    public interface IJwtService 
    {
         string GenerateToken(Guid userId);
         ClaimsPrincipal? ValidateToken(string token);
         Guid? GetUserIdFromToken(string token);
    }
}
