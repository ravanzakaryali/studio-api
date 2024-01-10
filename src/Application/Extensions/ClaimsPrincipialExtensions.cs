using System.Security.Claims;

namespace Space.Application.Extensions;

public static class ClaimsPrincipialExtensions
{
    public static string? GetLoginUserId(this ClaimsPrincipal principal)
    {
        if (principal == null)
            throw new ArgumentNullException(nameof(principal));

        return principal.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public static string? GetLoginUserName(this ClaimsPrincipal principal)
    {
        if (principal == null)
            throw new ArgumentNullException(nameof(principal));

        return principal.FindFirstValue(ClaimTypes.Name);
    }

    public static string? GetLoginUserEmail(this ClaimsPrincipal principal)
    {
        if (principal == null)
            throw new ArgumentNullException(nameof(principal));

        return principal.FindFirstValue(ClaimTypes.Email);
    }
}
