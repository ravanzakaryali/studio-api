namespace Space.Application.Handlers;

public class UpdateWorkerAppModulesAccessCommand : IRequest
{
    public int WorkerId { get; set; }
    public IEnumerable<UpdatePermissionAppModuleDto> PermissionAccesses { get; set; } = null!;
}

internal class UpdateWorkerAppModulesAccessCommandHandler : IRequestHandler<UpdateWorkerAppModulesAccessCommand>
{
    readonly ISpaceDbContext _spaceDbContext;
    public UpdateWorkerAppModulesAccessCommandHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }
    public async Task Handle(UpdateWorkerAppModulesAccessCommand request, CancellationToken cancellationToken)
    {
        Worker? worker = await _spaceDbContext.Workers.FindAsync(request.WorkerId)
                    ?? throw new NotFoundException(nameof(Worker), request.WorkerId);

        IEnumerable<int> targetModuleIds = request.PermissionAccesses.Select(p => p.Id);

        List<ApplicationModule> modulesToUpdate = await _spaceDbContext.ApplicationModules
                                .Include(c => c.WorkerPermissionLevelAppModules)
                                .Where(c => targetModuleIds.Contains(c.Id))
                                .ToListAsync(cancellationToken);

        foreach (var moduleDto in request.PermissionAccesses)
        {
            var module = modulesToUpdate.FirstOrDefault(m => m.Id == moduleDto.Id);
            if (module == null)
            {
                continue;
            }

            foreach (PermissionAccessLevelDto accessDto in moduleDto.Accesses)
            {
                WorkerPermissionLevelAppModule? existingPermission = module.WorkerPermissionLevelAppModules
                    .FirstOrDefault(p => p.PermissionLevelId == accessDto.Id && p.WorkerId == worker.Id);

                if (accessDto.IsAccess)
                {
                    if (existingPermission == null)
                    {
                        WorkerPermissionLevelAppModule newPermission = new()
                        {
                            WorkerId = worker.Id,
                            PermissionLevelId = accessDto.Id,
                            ApplicationModuleId = module.Id
                        };
                        _spaceDbContext.WorkerPermissionLevelAppModules.Add(newPermission);
                    }
                }
                else
                {
                    if (existingPermission != null)
                    {
                        _spaceDbContext.WorkerPermissionLevelAppModules.Remove(existingPermission);
                    }
                }
            }
        }
    }
}