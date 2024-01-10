using System.Security.Claims;

namespace Space.Application.Abstractions;

public interface ITokenService
{
    Token GenerateToken(User user, TimeSpan time, IList<string>? roles = null);
    string GenerateRefreshToken();
    string GenerateVerificationCode(int length = 6);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
