namespace Space.Application.Handlers;

public class GetWorkerAppModulesAccessQuery : IRequest<IEnumerable<GetAppModuleResponse>>
{
    public GetWorkerAppModulesAccessQuery(int workerId)
    {
        WorkerId = workerId;
    }
    public int WorkerId { get; }
}
internal class GetWorkerAppModulesAccessQueryHandler : IRequestHandler<GetWorkerAppModulesAccessQuery, IEnumerable<GetAppModuleResponse>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetWorkerAppModulesAccessQueryHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetAppModuleResponse>> Handle(GetWorkerAppModulesAccessQuery request, CancellationToken cancellationToken)
    {
        List<ApplicationModule> applicationModules = await _spaceDbContext.ApplicationModules
                    .Include(c => c.SubModules)
                    .Include(c => c.WorkerPermissionLevelAppModules)
                    .ThenInclude(c => c.PermissionLevel)
                    .ToListAsync(cancellationToken);

        List<PermissionLevel> permissionLevels = await _spaceDbContext.PermissionLevels.ToListAsync(cancellationToken);

        List<GetAppModuleResponse> appModules = applicationModules.Where(m => m.ParentModuleId == null)
                    .Select(m => MapToDto(m, request.WorkerId, applicationModules, permissionLevels)).ToList();

        return appModules;
    }

    private GetAppModuleResponse MapToDto(ApplicationModule module, int workerId, List<ApplicationModule> allModules, List<PermissionLevel> permissionLevels)
    {
        GetAppModuleResponse dto = new()
        {
            Id = module.Id,
            Name = module.Name,
            SubAppModules = allModules.Where(m => m.ParentModuleId == module.Id)
                                      .Select(m => MapToDto(m, workerId, allModules, permissionLevels)).ToList(),
            PermissionAccesses = permissionLevels.Select(pl => new GetPermissionAccessDto
            {
                Id = pl.Id,
                Name = pl.Name,
                IsAccess = module.WorkerPermissionLevelAppModules.Any(pgplam => pgplam.Id == workerId && pgplam.PermissionLevelId == pl.Id)
            }).ToList()
        };
        return dto;
    }
}
