using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Space.WebAPI.Middlewares;

public class TokenAutheticationMiddlewares
{
    private readonly RequestDelegate _next;

    public TokenAutheticationMiddlewares(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext.Request.Cookies.TryGetValue("token", out string? token))
        {
            httpContext.Request.Headers.Add("Authorization", "Bearer " + token);
        }
        await _next(httpContext);
    }
}
public static class TokenAutheticationMiddelwareExtensions
{
    public static IApplicationBuilder UseTokenAuthetication(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TokenAutheticationMiddlewares>();
    }
}