namespace Space.Application.Handlers;
public class RemoveWorkerFromPermissionGroupCommand : IRequest
{
    public int PermissionGroupId { get; set; }
    public int WorkerId { get; set; }
}
public class RemoveWorkerFromPermissionGroupCommandHandler : IRequestHandler<RemoveWorkerFromPermissionGroupCommand>
{
    readonly ISpaceDbContext _spaceDbContext;

    public RemoveWorkerFromPermissionGroupCommandHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }
    public async Task Handle(RemoveWorkerFromPermissionGroupCommand request, CancellationToken cancellationToken)
    {
        PermissionGroup? permissionGroup = await _spaceDbContext.PermissionGroups
                                                            .Include(pg => pg.Workers)
                                                            .FirstOrDefaultAsync(pg => pg.Id == request.PermissionGroupId, cancellationToken)
                                                                ?? throw new NotFoundException(nameof(PermissionGroup), request.PermissionGroupId);

        Worker? worker = await _spaceDbContext.Workers.FirstOrDefaultAsync(w => w.Id == request.WorkerId, cancellationToken)
                        ?? throw new NotFoundException(nameof(Worker), request.WorkerId);

        permissionGroup.Workers.Remove(worker);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}