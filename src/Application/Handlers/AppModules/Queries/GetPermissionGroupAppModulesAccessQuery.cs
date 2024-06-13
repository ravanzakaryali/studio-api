using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Space.Application.Handlers.Queries;


public class GetPermissionGroupAppModulesAccessQuery : IRequest<GetAppModuleResponseDto>
{
    public int GroupId { get; set; }
}

internal class GetPermissionGroupAppModulesAccessQueryHandler : IRequestHandler<GetPermissionGroupAppModulesAccessQuery, GetAppModuleResponseDto>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetPermissionGroupAppModulesAccessQueryHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<GetAppModuleResponseDto> Handle(GetPermissionGroupAppModulesAccessQuery request, CancellationToken cancellationToken)
    {

        PermissionGroup permissionGroup = await _spaceDbContext.PermissionGroups.FindAsync(request.GroupId)
                    ?? throw new NotFoundException(nameof(PermissionGroup), request.GroupId);

        // Load all necessary data with includes
        List<ApplicationModule> applicationModules = await _spaceDbContext.ApplicationModules
                    .Include(c => c.SubModules)
                    .Include(c => c.PermissionGroupPermissionLevelAppModules)
                    .ThenInclude(c => c.PermissionLevel)
                    .ToListAsync(cancellationToken);

        List<PermissionLevel> permissionLevels = await _spaceDbContext.PermissionLevels.ToListAsync(cancellationToken);

        List<GetAppModuleResponse> appModules = applicationModules.Where(m => m.ParentModuleId == null)
                    .Select(m => MapToDto(m, request.GroupId, applicationModules, permissionLevels)).ToList();

        return new GetAppModuleResponseDto()
        {
            Id = permissionGroup.Id,
            Name = permissionGroup.Name,
            AppModules = appModules
        };
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
