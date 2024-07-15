using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Space.Application.Extensions;

namespace Space.Application.Handlers;

public record UserLoginQuery : IRequest<GetUserResponseDto> { }
internal class UserLoginQueryHandler : IRequestHandler<UserLoginQuery, GetUserResponseDto>
{

    readonly IHttpContextAccessor _contextAccessor;
    readonly UserManager<User> _userManager;
    readonly ISpaceDbContext _context;

    public UserLoginQueryHandler(IHttpContextAccessor contextAccessor, UserManager<User> userManager, ISpaceDbContext context)
    {
        _contextAccessor = contextAccessor;
        _userManager = userManager;
        _context = context;
    }

    public async Task<GetUserResponseDto> Handle(UserLoginQuery request, CancellationToken cancellationToken)
    {
        string userId = _contextAccessor.HttpContext?.User?.GetLoginUserId()
           ?? throw new UnauthorizedAccessException();


        Worker worker = await _context.Workers
                        .Where(w => w.Id == int.Parse(userId))
                        .Include(w => w.UserRoles)
                        .ThenInclude(w => w.Role)
                        .Include(w => w.WorkerPermissionLevelAppModules)
                        .ThenInclude(w => w.ApplicationModule)
                        .Include(w => w.WorkerPermissionLevelAppModules)
                        .ThenInclude(w => w.PermissionLevel)
                        .ThenInclude(w => w.PermissionAccesses)
                        .Include(w => w.PermissionGroups)
                        .ThenInclude(w => w.PermissionGroupPermissionLevelAppModules)
                        .ThenInclude(w => w.ApplicationModule)
                        .ThenInclude(w => w.PermissionGroupPermissionLevelAppModules)
                        .ThenInclude(w => w.PermissionLevel)
                        .ThenInclude(w => w.PermissionAccesses)
                        .FirstOrDefaultAsync(cancellationToken: cancellationToken)
            ?? throw new UnauthorizedAccessException();

        GetUserResponseDto response = new()
        {
            Id = int.Parse(userId),
            Surname = worker.Surname,
            Name = worker.Name,
            RoleName = worker.UserRoles.Select(u => u.Role.Name).ToList()
        };

        foreach (WorkerPermissionLevelAppModule item in worker.WorkerPermissionLevelAppModules)
        {
            response.Permissions.Add(new LoginUserPermissionDto()
            {
                AppModuleId = item.ApplicationModuleId,
                Name = item.ApplicationModule.Name,
                NormalizedName = item.ApplicationModule.NormalizedName,
                Permissions = item.PermissionLevel.PermissionAccesses.Select(p => p.Name).ToList()
            });
        }
        foreach (PermissionGroup item in worker.PermissionGroups)
        {
            foreach (PermissionGroupPermissionLevelAppModule appModule in item.PermissionGroupPermissionLevelAppModules)
            {
                response.Permissions.Add(new LoginUserPermissionDto()
                {
                    AppModuleId = appModule.ApplicationModuleId,
                    Name = appModule.ApplicationModule.Name,
                    NormalizedName = appModule.ApplicationModule.NormalizedName,
                    Permissions = appModule.PermissionLevel.PermissionAccesses.Select(p => p.Name).ToList()
                });
            }
        }

        response.Permissions = response.Permissions.OrderByDescending(p => p.Permissions.Count).DistinctBy(p => p.AppModuleId).ToList();
        return response;
    }
}
