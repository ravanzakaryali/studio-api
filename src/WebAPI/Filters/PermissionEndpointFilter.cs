

using Azure;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NPOI.SS.Formula;
using Space.Application.Extensions;


namespace Space.WebAPI.Filters;

public class PermissionEndpointFilter : IAsyncActionFilter
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IHttpContextAccessor _contextAccessor;

    public PermissionEndpointFilter(
                ISpaceDbContext spaceDbContext,
                IHttpContextAccessor contextAccessor)
    {
        _spaceDbContext = spaceDbContext;
        _contextAccessor = contextAccessor;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        Endpoint? endpoint = context.HttpContext.GetEndpoint();
        if (endpoint == null)
        {
            await next();
            return;
        }
        ControllerActionDescriptor? descriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();

        string? pattern = descriptor?.AttributeRouteInfo?.Template;
        string httpMethod = descriptor?.ActionConstraints?.OfType<HttpMethodActionConstraint>().FirstOrDefault()?.HttpMethods.FirstOrDefault() ?? "GET";

        bool? isAuth = descriptor?.EndpointMetadata.Any(c => c is AuthorizeAttribute);

        E.Endpoint? endpointDb = await _spaceDbContext.Endpoints
            .Where(c => c.Path == pattern && c.HttpMethod == httpMethod)
            .Include(c => c.EndpointAccesses)
            .ThenInclude(c => c.ApplicationModule)
            .Include(c => c.EndpointAccesses)
            .ThenInclude(c => c.PermissionAccess)
            .FirstOrDefaultAsync();

        if (endpointDb == null)
        {
            context.HttpContext.Response.StatusCode = 404;
            await context.HttpContext.Response.WriteAsync("Endpoint not found");
            return;
        }



        string? userId = _contextAccessor.HttpContext?.User.GetLoginUserId();

        if (userId == null)
        {
            if (isAuth is not null and true)
            {
                context.HttpContext.Response.StatusCode = 401;
                await context.HttpContext.Response.WriteAsync("Unauthorized");
                return;
            }
            else
            {
                await next();
                return;
            }


        }
        E.Worker? worker = await _spaceDbContext.Workers
            .Include(c => c.PermissionGroups)
            .ThenInclude(c => c.PermissionGroupPermissionLevelAppModules)
            .ThenInclude(c => c.PermissionLevel)
            .ThenInclude(c => c.PermissionAccesses)
            .Include(c => c.WorkerPermissionLevelAppModules)
            .ThenInclude(c => c.ApplicationModule)
            .FirstOrDefaultAsync(c => c.Id == int.Parse(userId));

        if (worker == null)
        {
            context.HttpContext.Response.StatusCode = 401;
            await context.HttpContext.Response.WriteAsync("Unauthorized");
            return;
        }

        if (endpointDb.EndpointAccesses.Any(c => c.ApplicationModuleId == null))
        {
            await next();
            return;
        }


        ICollection<E.PermissionGroup> permissiongroups = worker.PermissionGroups;


        IEnumerable<E.EndpointAccess> accessList = from endPointItem in endpointDb.EndpointAccesses
                                                   from permissionGroup in permissiongroups
                                                   from levelAppModule in permissionGroup.PermissionGroupPermissionLevelAppModules
                                                   where levelAppModule.PermissionLevel.PermissionAccesses
                                                         .Any(c => c.Id == endPointItem.PermissionAccessId && endPointItem.ApplicationModuleId == levelAppModule.ApplicationModuleId)
                                                   select endPointItem;

        // if (accessList.Any())
        // {
        await next();
        return;
        // }



        // context.HttpContext.Response.StatusCode = 403;
        // await context.HttpContext.Response.WriteAsync("Forbidden");
        // return;
    }
}