namespace Space.Application.Handlers;

public class SetAccessToPermissionGroupCommand : IRequest
{
    public SetAccessToPermissionGroupCommand()
    {
        AppModulesAccess = new List<SetAccessToPermissionGroupAndWorkerDto>();
    }
    public int PermissionGroupId { get; set; }
    public IEnumerable<SetAccessToPermissionGroupAndWorkerDto> AppModulesAccess { get; set; }
}
internal class SetAccessToPermissionGroupCommandHandler : IRequestHandler<SetAccessToPermissionGroupCommand>
{
    readonly ISpaceDbContext _spaceDbContext;

    public SetAccessToPermissionGroupCommandHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }
    public async Task Handle(SetAccessToPermissionGroupCommand request, CancellationToken cancellationToken)
    {
        PermissionGroup? permissionGroup = await _spaceDbContext.PermissionGroups
            .Include(pg => pg.Workers)
                .ThenInclude(w => w.WorkerPermissionLevelAppModules)
            .Include(pg => pg.PermissionGroupPermissionLevelAppModules)
            .FirstOrDefaultAsync(pg => pg.Id == request.PermissionGroupId, cancellationToken)
            ?? throw new NotFoundException(nameof(PermissionGroup), request.PermissionGroupId);

        permissionGroup.PermissionGroupPermissionLevelAppModules.Clear();
        foreach (Worker worker in permissionGroup.Workers)
        {
            worker.WorkerPermissionLevelAppModules.Clear();
        }

        List<ApplicationModule> applicationModules = await _spaceDbContext.ApplicationModules
            .ToListAsync(cancellationToken);

        List<PermissionLevel> permissionLevels = await _spaceDbContext.PermissionLevels
            .ToListAsync(cancellationToken);

        //Todo: Refactor this code

        foreach (SetAccessToPermissionGroupAndWorkerDto appModule in request.AppModulesAccess)
        {
            _ = applicationModules.FirstOrDefault(am => am.Id == appModule.AppModuleId)
                ?? throw new NotFoundException(nameof(ApplicationModule), appModule.AppModuleId);

            foreach (PermissionGroupSetPermissionLevelDto permissionLevel in appModule.PermissionLevels)
            {
                _ = permissionLevels.FirstOrDefault(pl => pl.Id == permissionLevel.PermissionLevelId)
                    ?? throw new NotFoundException(nameof(PermissionLevel), permissionLevel.PermissionLevelId);

                if (permissionLevel.IsAccess)
                {
                    PermissionGroupPermissionLevelAppModule permissionGroupPermissionLevelAppModule = new()
                    {
                        PermissionGroupId = permissionGroup.Id,
                        ApplicationModuleId = appModule.AppModuleId,
                        PermissionLevelId = permissionLevel.PermissionLevelId
                    };
                    permissionGroup.PermissionGroupPermissionLevelAppModules.Add(permissionGroupPermissionLevelAppModule);
                }
                foreach (Worker worker in permissionGroup.Workers)
                {

                    if (permissionLevel.IsAccess)
                    {
                        WorkerPermissionLevelAppModule workerPermissionLevelAppModule = new()
                        {
                            WorkerId = worker.Id,
                            ApplicationModuleId = appModule.AppModuleId,
                            PermissionLevelId = permissionLevel.PermissionLevelId
                        };
                    }
                }
            }

        }

        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}