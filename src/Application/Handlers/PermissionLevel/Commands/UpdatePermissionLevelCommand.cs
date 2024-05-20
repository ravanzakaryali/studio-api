namespace Space.Application.Handlers.Queries;

public class UpdatePermissionLevelCommand : IRequest
{
    public IEnumerable<UpdatePermissionLevelDto> PermissionAccesses { get; set; } = null!;
}
internal class UpdatePermissionLevelCommandHandler : IRequestHandler<UpdatePermissionLevelCommand>
{

    readonly ISpaceDbContext _spaceDbContext;
    public UpdatePermissionLevelCommandHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(UpdatePermissionLevelCommand request, CancellationToken cancellationToken)
    {
        List<PermissionAccess> permissionAccesses = await _spaceDbContext.PermissionAccesses
                                .Include(c => c.PermissionLevels)
                                .ToListAsync(cancellationToken);

        List<PermissionLevel> permissionLevels = await _spaceDbContext.PermissionLevels.ToListAsync(cancellationToken: cancellationToken);

        foreach (UpdatePermissionLevelDto permissionAccess in request.PermissionAccesses)
        {
            PermissionLevel? permissionLevel = permissionLevels.Where(c => c.Id == permissionAccess.Id).FirstOrDefault()
                        ?? throw new NotFoundException(nameof(PermissionLevel), permissionAccess.Id);


            permissionLevel.PermissionAccesses.Clear();
            foreach (PermissionAccessLevelDto access in permissionAccess.Accesses)
            {
                PermissionAccess permissionAccessDb = permissionAccesses.Where(c => c.Id == access.Id).FirstOrDefault()
                     ?? throw new NotFoundException(nameof(PermissionAccess), permissionAccess.Id);

                if (access.IsAccess)
                {
                    permissionAccessDb.PermissionLevels.Add(permissionLevel);
                }
            }

        }
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}