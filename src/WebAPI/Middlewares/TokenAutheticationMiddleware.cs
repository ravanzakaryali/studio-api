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

public class TokenAutheticationMiddlewares
{
    private readonly RequestDelegate _next;

    public TokenAutheticationMiddlewares(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext httpContext, ITokenService tokenService, IUnitOfWork unitOfWork)
    {
        if (httpContext.Request.Cookies.TryGetValue("token", out string? token))
        {
            if (token != null)
            {
                ClaimsPrincipal claimsPrincipal = tokenService.GetPrincipalFromExpiredToken(token);

                string? tokenExpiration = claimsPrincipal.Claims
                .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;

                if (!string.IsNullOrEmpty(tokenExpiration))
                {
                    DateTime expiration = DateTimeOffset.FromUnixTimeSeconds(long.Parse(tokenExpiration)).UtcDateTime;

                    string? loginUserId = claimsPrincipal.GetLoginUserId();
                    if (expiration <= DateTime.UtcNow && loginUserId != null)
                    {
                        User? user = await unitOfWork.UserService.FindById(new Guid(loginUserId));
                        IList<string> roles = await unitOfWork.RoleService.GetRolesByUser(user);


                        Token newAccessToken = tokenService.GenerateToken(user, TimeSpan.FromMinutes(45), roles);
                        httpContext.Request.Headers.Add("Authorization", "Bearer " + newAccessToken);
                        httpContext?.Response.Cookies.Append("token", newAccessToken.AccessToken, new CookieOptions
                        {
                            Expires = newAccessToken.Expires,
                            HttpOnly = true,
                            SameSite = SameSiteMode.None,
                            Secure = true,
                        });
                    }
                }
            }
            if (!httpContext!.Request.Headers.TryGetValue("Authorization", out StringValues value))
            {
                httpContext.Request.Headers.Add("Authorization", "Bearer " + token);
            }
        }
        await _next(httpContext!);
    }
}
public static class TokenAutheticationMiddelwareExtensions
{
    public static IApplicationBuilder UseTokenAuthetication(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TokenAutheticationMiddlewares>();
    }
}