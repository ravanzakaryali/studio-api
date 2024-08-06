namespace Space.Application.Handlers;

public class UpdatePermissionGroupAppModulesAccessCommand : IRequest
{
    public int GroupId { get; set; }
    public IEnumerable<UpdatePermissionAppModuleDto> PermissionAccesses { get; set; } = null!;
}

internal class UpdatePermissionGroupAppModulesAccessCommandHandler : IRequestHandler<UpdatePermissionGroupAppModulesAccessCommand>
{
    readonly ISpaceDbContext _spaceDbContext;
    public UpdatePermissionGroupAppModulesAccessCommandHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }
    public async Task Handle(UpdatePermissionGroupAppModulesAccessCommand request, CancellationToken cancellationToken)
    {
        PermissionGroup? permissionGroup = await _spaceDbContext.PermissionGroups.FindAsync(request.GroupId)
                    ?? throw new NotFoundException(nameof(PermissionGroup), request.GroupId);

        IEnumerable<int> targetModuleIds = request.PermissionAccesses.Select(p => p.Id);

        List<ApplicationModule> modulesToUpdate = await _spaceDbContext.ApplicationModules
                                .Include(c => c.PermissionGroupPermissionLevelAppModules)
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
                PermissionGroupPermissionLevelAppModule? existingPermission = module.PermissionGroupPermissionLevelAppModules
                    .FirstOrDefault(p => p.PermissionLevelId == accessDto.Id && p.PermissionGroupId == request.GroupId);

                if (accessDto.IsAccess)
                {
                    if (existingPermission == null)
                    {
                        PermissionGroupPermissionLevelAppModule newPermission = new PermissionGroupPermissionLevelAppModule
                        {
                            PermissionGroupId = request.GroupId,
                            PermissionLevelId = accessDto.Id,
                            ApplicationModuleId = module.Id
                        };
                        _spaceDbContext.PermissionGroupPermissionLevelAppModules.Add(newPermission);
                    }
                }
                else
                {
                    if (existingPermission != null)
                    {
                        _spaceDbContext.PermissionGroupPermissionLevelAppModules.Remove(existingPermission);
                    }
                }
            }
        }

        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}

