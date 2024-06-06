namespace Space.Application.Handlers.Queries;


public class GetPermissionGroupAppModulesAccessQuery : IRequest<IEnumerable<GetAppModuleResponse>>
{
    public int GroupId { get; set; }
}

internal class GetPermissionGroupAppModulesAccessQueryHandler : IRequestHandler<GetPermissionGroupAppModulesAccessQuery, IEnumerable<GetAppModuleResponse>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetPermissionGroupAppModulesAccessQueryHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetAppModuleResponse>> Handle(GetPermissionGroupAppModulesAccessQuery request, CancellationToken cancellationToken)
    {
        // Load all necessary data with includes
        var applicationModules = await _spaceDbContext.ApplicationModules
                    .Include(c => c.SubModules)
                    .Include(c => c.PermissionGroupPermissionLevelAppModules)
                    .ThenInclude(c => c.PermissionLevel)
                    .ToListAsync(cancellationToken);

        var permissionLevels = await _spaceDbContext.PermissionLevels.ToListAsync(cancellationToken);

        // Map to DTO
        var appModules = applicationModules.Where(m => m.ParentModuleId == null)
                    .Select(m => MapToDto(m, request.GroupId, applicationModules, permissionLevels)).ToList();

        return appModules;
    }

    private GetAppModuleResponse MapToDto(ApplicationModule module, int groupId, List<ApplicationModule> allModules, List<PermissionLevel> permissionLevels)
    {
        var dto = new GetAppModuleResponse
        {
            Id = module.Id,
            Name = module.Name,
            SubAppModules = allModules.Where(m => m.ParentModuleId == module.Id)
                                      .Select(m => MapToDto(m, groupId, allModules, permissionLevels)).ToList(),
            PermissionAccesses = permissionLevels.Select(pl => new GetPermissionAccessDto
            {
                Id = pl.Id,
                Name = pl.Name,
                IsAccess = module.PermissionGroupPermissionLevelAppModules.Any(pgplam => pgplam.PermissionGroupId == groupId && pgplam.PermissionLevelId == pl.Id)
            }).ToList()
        };
        return dto;
    }
}
