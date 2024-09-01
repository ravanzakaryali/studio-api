using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Space.Domain.Enums;

namespace Space.WebAPI.Middlewares
{
    public class EndpointScannerMiddleware
    {
        private readonly RequestDelegate _next;

        public EndpointScannerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IActionDescriptorCollectionProvider actionDescriptorCollectionProvider, ISpaceDbContext spaceDbContext)
        {
            IReadOnlyList<ActionDescriptor> endpoints = actionDescriptorCollectionProvider.ActionDescriptors.Items;
            List<E.Endpoint> endpointsDb = await spaceDbContext.Endpoints.ToListAsync();

            List<string> currentEndpoints = new();

            foreach (var endpoint in endpoints)
            {
                if (endpoint is ControllerActionDescriptor controllerActionDescriptor)
                {
                    string? endpointPath = controllerActionDescriptor.AttributeRouteInfo?.Template;
                    string httpMethod = controllerActionDescriptor.ActionConstraints?.OfType<HttpMethodActionConstraint>().FirstOrDefault()?.HttpMethods.FirstOrDefault() ?? "GET";

                    if (endpointPath != null)
                    {
                        E.Endpoint endpointEntity = new()
                        {
                            Path = endpointPath,
                            HttpMethod = httpMethod
                        };

                        bool isExist = endpointsDb.Any(ep => ep.Path == endpointPath && ep.HttpMethod == endpointEntity.HttpMethod);
                        if (!isExist)
                        {
                            await spaceDbContext.Endpoints.AddAsync(endpointEntity);
                        }

                        currentEndpoints.Add($"{endpointPath}-{httpMethod}");
                    }
                }
            }

            //foreach (var endpointDb in endpointsDb)
            //{
            //    if (!currentEndpoints.Contains($"{endpointDb.Path}-{endpointDb.HttpMethod}"))
            //    {
            //        spaceDbContext.Endpoints.Remove(endpointDb);
            //    }
            //}

            await spaceDbContext.SaveChangesAsync();
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
}