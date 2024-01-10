using AspNetCoreRateLimit;
using Microsoft.Extensions.Options;

namespace Space.WebAPI.Middlewares;

/// <summary>
/// Rate limit middleware
/// </summary>
public class RateLimitMiddleware : ClientRateLimitMiddleware
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RateLimitMiddleware"/> class with the specified dependencies.
    /// </summary>
    /// <param name="next">The next middleware in the request processing pipeline.</param>
    /// <param name="processingStrategy">The rate limit processing strategy.</param>
    /// <param name="options">The client rate limit options.</param>
    /// <param name="policyStore">The client policy store.</param>
    /// <param name="config">The rate limit configuration.</param>
    /// <param name="logger">The logger for rate limit middleware.</param>
    public RateLimitMiddleware(RequestDelegate next,
           IProcessingStrategy processingStrategy,
           IOptions<ClientRateLimitOptions> options,
           IClientPolicyStore policyStore,
           IRateLimitConfiguration config,
           ILogger<ClientRateLimitMiddleware> logger) :
           base(next, processingStrategy, options, policyStore, config, logger)

    {
    }

    /// <summary>
    /// Handles the response when the API calls quota for a client is exceeded.
    /// </summary>
    /// <param name="httpContext">The HTTP context for the current request.</param>
    /// <param name="rule">The rate limit rule that was exceeded.</param>
    /// <param name="retryAfter">The time in seconds after which the client should retry the request.</param>
    /// <returns>A task representing the asynchronous response handling.</returns>
    public override Task ReturnQuotaExceededResponse
   (HttpContext httpContext, RateLimitRule rule, string retryAfter)
    {
        // Get the path of the current HTTP request
        string? path = httpContext?.Request?.Path.Value;

        // Serialize the response message
        var result = JsonSerializer.Serialize("API calls quota exceeded!");

        // Ensure that the HTTP context is not null
        if (httpContext == null)
        {
            throw new NullReferenceException("Http context is null in Rate Limit Middleware");
        }
        // Set the 'Retry-After' header to indicate when the client should retry the request
        httpContext.Response.Headers["Retry-After"] = retryAfter;

        // Set the HTTP status code to indicate that the quota has been exceeded (429 Too Many Requests)
        httpContext.Response.StatusCode = 429;

        // Set the content type of the response to JSON
        httpContext.Response.ContentType = "application/json";

        // Write the serialized response message to the response body
        return httpContext.Response.WriteAsync(result);
    }
    private void WriteQuotaExceededResponseMetadata
    (string requestPath, string retryAfter, int statusCode = 429)
    {
        //Todo: Log write data to the database
    }
}

/// <summary>
/// Provides extension methods for configuring rate limit middleware in the application pipeline.
/// </summary>
public static class RateLimitMiddlewareExtensions
{
    /// <summary>
    /// Adds the rate limit middleware to the application pipeline.
    /// </summary>
    /// <param name="builder">The <see cref="IApplicationBuilder"/> instance.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> instance with rate limit middleware added.</returns>
    public static IApplicationBuilder UseRateLimit(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RateLimitMiddleware>();
    }
}