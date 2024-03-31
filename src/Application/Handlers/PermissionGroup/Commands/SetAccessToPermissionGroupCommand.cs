namespace Space.Application.Handlers;

public class SetAccessToPermissionGroupCommand : IRequest
{
    public SetAccessToPermissionGroupCommand()
    {
        PermissionLevels = new List<SetAccessToPermissionGroupDto>();
    }
    public int PermissionGroupId { get; set; }
    public IEnumerable<SetAccessToPermissionGroupDto> PermissionLevels { get; set; }
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
            .Include(pg => pg.ApplicationModules)
            .FirstOrDefaultAsync(pg => pg.Id == request.PermissionGroupId, cancellationToken)
            ?? throw new NotFoundException(nameof(PermissionGroup), request.PermissionGroupId);

        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}