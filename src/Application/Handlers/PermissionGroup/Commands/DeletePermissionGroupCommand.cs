namespace Space.Application.Handlers;
public class DeletePermissionGroupCommand : IRequest
{
    public int Id { get; set; }
}
public class DeletePermissionGroupCommandHandler : IRequestHandler<DeletePermissionGroupCommand>
{
    readonly ISpaceDbContext _spaceDbContext;

    public DeletePermissionGroupCommandHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(DeletePermissionGroupCommand request, CancellationToken cancellationToken)
    {
        PermissionGroup? permissionGroup = await _spaceDbContext.PermissionGroups
                                            .FirstOrDefaultAsync(pg => pg.Id == request.Id, cancellationToken)
                                            ?? throw new NotFoundException(nameof(PermissionGroup), request.Id);

        _spaceDbContext.PermissionGroups.Remove(permissionGroup);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}