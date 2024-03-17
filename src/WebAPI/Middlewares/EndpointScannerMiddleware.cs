using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Space.WebAPI.Middlewares;
public class EndpointScannerMiddleware
{
    private readonly RequestDelegate _next;
    public EndpointScannerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext httpContext, IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider)
    {
        IReadOnlyList<ActionDescriptor> endpoints = _actionDescriptorCollectionProvider.ActionDescriptors.Items;

        foreach (var endpoint in endpoints)
        {
            if (endpoint is ControllerActionDescriptor controllerActionDescriptor)
            {
                string controllerName = controllerActionDescriptor.ControllerName;
                string methodName = controllerActionDescriptor.ActionName;
                string? endpointPath = controllerActionDescriptor.AttributeRouteInfo?.Template;
                string httpMethod = controllerActionDescriptor.ActionConstraints?.OfType<HttpMethodActionConstraint>().FirstOrDefault()?.HttpMethods.FirstOrDefault() ?? "GET";
                
            }
        }
        await _next(httpContext);
    }
}

public static class EndpointScannerMiddlewareExtensions
{
    public static IApplicationBuilder UseEndpointScanner(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<EndpointScannerMiddleware>();
    }
}