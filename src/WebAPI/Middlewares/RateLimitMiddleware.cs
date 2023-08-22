using AspNetCoreRateLimit;
using Microsoft.Extensions.Options;

namespace Space.WebAPI.Middlewares;

public class RateLimitMiddleware : ClientRateLimitMiddleware
{
    public RateLimitMiddleware(RequestDelegate next,
           IProcessingStrategy processingStrategy,
           IOptions<ClientRateLimitOptions> options,
           IClientPolicyStore policyStore,
           IRateLimitConfiguration config,
           ILogger<ClientRateLimitMiddleware> logger) :
           base(next, processingStrategy, options, policyStore, config, logger)

    {

    }

    public override Task ReturnQuotaExceededResponse
   (HttpContext httpContext, RateLimitRule rule, string retryAfter)
    {
        string? path = httpContext?.Request?.Path.Value;
        var result = JsonSerializer.Serialize("API calls quota exceeded!");
        httpContext.Response.Headers["Retry-After"] = retryAfter;
        httpContext.Response.StatusCode = 429;
        httpContext.Response.ContentType = "application/json";
        return httpContext.Response.WriteAsync(result);
    }
    private void WriteQuotaExceededResponseMetadata
    (string requestPath, string retryAfter, int statusCode = 429)
    {
        //Code to write data to the database
    }
}

public static class RateLimitMiddlewareExtensions
{
    public static IApplicationBuilder UseRateLimit(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RateLimitMiddleware>();
    }
}