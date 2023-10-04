using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Space.Application.Abstractions;
using Space.Application.Extensions;
using Space.Domain.Entities;
using Space.Infrastructure.Services;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace Space.WebAPI.Middlewares;

/// <summary>
/// JWT token authetication middlewares
/// </summary>
public class TokenAutheticationMiddlewares
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Initializes a new instance of the TokenAuthenticationMiddleware class.
    /// </summary>
    /// <param name="next">The delegate representing the next middleware in the pipeline.</param>
    public TokenAutheticationMiddlewares(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Handles the authentication token in the request by checking for a token cookie, validating and refreshing the token if necessary, and adding it to the request headers.
    /// </summary>
    /// <param name="httpContext">The HTTP context for the current request.</param>
    /// <param name="tokenService">The service responsible for token operations.</param>
    /// <param name="unitOfWork">The unit of work for database access.</param>
    public async Task InvokeAsync(HttpContext httpContext, ITokenService tokenService, IUnitOfWork unitOfWork)
    {
        // Check for the presence of a token cookie in the request
        if (httpContext.Request.Cookies.TryGetValue("token", out string? token))
        {
            if (token != null)
            {
                // Get the claims principal from the expired token
                ClaimsPrincipal claimsPrincipal = tokenService.GetPrincipalFromExpiredToken(token);

                // Extract token expiration time
                string? tokenExpiration = claimsPrincipal.Claims
                    .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;

                if (!string.IsNullOrEmpty(tokenExpiration))
                {
                    // Convert token expiration time to DateTime
                    DateTime expiration = DateTimeOffset.FromUnixTimeSeconds(long.Parse(tokenExpiration)).UtcDateTime;

                    // Get the login user ID from claims
                    string? loginUserId = claimsPrincipal.GetLoginUserId();
                    if (expiration <= DateTime.UtcNow && loginUserId != null)
                    {
                        // Find user roles and generate a new access token
                        User? user = await unitOfWork.UserService.FindById(new Guid(loginUserId));
                        IList<string> roles = await unitOfWork.RoleService.GetRolesByUser(user);

                        Token newAccessToken = tokenService.GenerateToken(user, TimeSpan.FromSeconds(10), roles);

                        // Set the new access token in a new cookie with extended expiration
                        httpContext.Response.Cookies.Append("token", newAccessToken.AccessToken, new CookieOptions
                        {
                            Expires = newAccessToken.Expires.AddDays(7),
                            HttpOnly = true,
                            SameSite = SameSiteMode.None,
                            Secure = true,
                        });

                        // Add the new token to the request headers
                        httpContext.Request.Headers.Add("Authorization", "Bearer " + newAccessToken);
                        List<Claim> claims = new()
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, user.UserName!),
                            new Claim(ClaimTypes.Email, user.Email!),
                        };
                        httpContext.User.AddIdentity(new ClaimsIdentity(claims));
                        // Continue processing the request with the updated token
                        await _next(httpContext);
                    }
                }
            }

            // Add the original token to the request headers
            httpContext.Request.Headers.Add("Authorization", "Bearer " + token);
        }

        // Continue processing the request
        await _next(httpContext);
    }

}
/// <summary>
/// Token Authetication Middleware
/// </summary>
public static class TokenAutheticationMiddelwareExtensions
{
    /// <summary>
    /// Token Authetication Builder
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseTokenAuthetication(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TokenAutheticationMiddlewares>();
    }
}