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
    public async Task InvokeAsync(HttpContext httpContext)
    {

        if (httpContext.Request.Cookies.TryGetValue("token", out string? token))
        {
            httpContext.Request.Headers.Add("Authorization", "Bearer " + token);
            httpContext.Response.Headers.Add("Authorization", "Bearer " + token);
        }

        await _next(httpContext);
    }

}

/// <summary>
/// 
/// </summary>
public class ChangeTokenAutheticationMiddlewares
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="next"></param>
    public ChangeTokenAutheticationMiddlewares(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="tokenService"></param>
    /// <param name="unitOfWork"></param>
    /// <returns></returns>
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
                        if (!int.TryParse(loginUserId, out int result))
                        {
                            httpContext.Response.Cookies.Append("token", "delete", new CookieOptions
                            {
                                Expires = DateTime.Now.AddDays(-1),
                                HttpOnly = false,
                                SameSite = SameSiteMode.None,
                                Secure = true,
                            });
                            await _next(httpContext);
                            return;
                        }
                        User? user = await unitOfWork.UserService.FindById(int.Parse(loginUserId));
                        IList<string> roles = await unitOfWork.RoleService.GetRolesByUser(user);

                        Token newAccessToken = tokenService.GenerateToken(user, TimeSpan.FromSeconds(10), roles);

                        httpContext.Response.Cookies.Append("token", newAccessToken.AccessToken, new CookieOptions
                        {
                            Expires = newAccessToken.Expires.AddDays(7),
                            HttpOnly = false,
                            SameSite = SameSiteMode.None,
                            Secure = true,
                        });
                        //httpContext.Request.Cookies = new KeyValuePair<string,string>("token", newAccessToken.AccessToken);

                        List<Claim> claims = new()
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, user.UserName!),
                            new Claim(ClaimTypes.Email, user.Email!),
                        };

                        httpContext.Request.Headers.Remove("Authorization");
                        httpContext.Response.Headers.Remove("Authorization");
                        httpContext.Response.Headers.Add("Authorization", "Bearer " + newAccessToken.AccessToken);
                        httpContext.Request.Headers.Add("Authorization", "Bearer " + newAccessToken.AccessToken);

                        httpContext.User.AddIdentity(new ClaimsIdentity(claims));
                    }
                }
            }
        }

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
    /// <summary>
    /// Change token Authetication
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseChangeTokenAuthetication(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ChangeTokenAutheticationMiddlewares>();
    }
}