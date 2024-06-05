namespace Space.Application.Handlers;
public class UpdatePermissionGroupCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}
public class UpdatePermissionGroupCommandHandler : IRequestHandler<UpdatePermissionGroupCommand>
{
    readonly ISpaceDbContext _spaceDbContext;

    public UpdatePermissionGroupCommandHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }
    public async Task Handle(UpdatePermissionGroupCommand request, CancellationToken cancellationToken)
    {

        PermissionGroup permissionGroup = await _spaceDbContext.PermissionGroups
            .FirstOrDefaultAsync(pg => pg.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(PermissionGroup), request.Id);

        permissionGroup.Name = request.Name;
        permissionGroup.Description = request.Description;

        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}