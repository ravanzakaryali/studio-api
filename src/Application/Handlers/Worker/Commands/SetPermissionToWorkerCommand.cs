namespace Space.Application.Handlers;
public class SetPermissionToWorkerCommand : IRequest
{
    public SetPermissionToWorkerCommand()
    {
        AppModulesAccess = new List<SetAccessToPermissionGroupAndWorkerDto>();
    }
    public int WorkerId { get; set; }
    public IEnumerable<SetAccessToPermissionGroupAndWorkerDto> AppModulesAccess { get; set; }
}
internal class SetPermissionToWorkerCommandHandler : IRequestHandler<SetPermissionToWorkerCommand>
{
    readonly ISpaceDbContext _spaceDbContext;

    public SetPermissionToWorkerCommandHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }
    public async Task Handle(SetPermissionToWorkerCommand request, CancellationToken cancellationToken)
    {
        Worker? worker = await _spaceDbContext.Workers
            .Include(w => w.WorkerPermissionLevelAppModules)
            .FirstOrDefaultAsync(w => w.Id == request.WorkerId, cancellationToken)
            ?? throw new NotFoundException(nameof(Worker), request.WorkerId);

        worker.WorkerPermissionLevelAppModules.Clear();

        List<ApplicationModule> applicationModules = await _spaceDbContext.ApplicationModules
            .ToListAsync(cancellationToken);

        List<PermissionLevel> permissionLevels = await _spaceDbContext.PermissionLevels
            .ToListAsync(cancellationToken);

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
                    WorkerPermissionLevelAppModule workerPermissionLevelAppModule = new()
                    {
                        WorkerId = worker.Id,
                        ApplicationModuleId = appModule.AppModuleId,
                        PermissionLevelId = permissionLevel.PermissionLevelId
                    };

                    worker.WorkerPermissionLevelAppModules.Add(workerPermissionLevelAppModule);
                }
            }
        }

        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}