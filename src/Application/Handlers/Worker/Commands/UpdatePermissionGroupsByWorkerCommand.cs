namespace Space.Application.Handlers;
public class UpdatePermissionGroupsByWorkerCommand : IRequest
{
    public UpdatePermissionGroupsByWorkerCommand()
    {
        PermissionGroups = new List<PermissionGroupId>();
    }
    public int WorkerId { get; set; }
    public IEnumerable<PermissionGroupId> PermissionGroups { get; set; }
}
internal class UpdatePermissionGroupsByWorkerCommandHandler : IRequestHandler<UpdatePermissionGroupsByWorkerCommand>
{
    readonly ISpaceDbContext _spaceDbContext;
    public UpdatePermissionGroupsByWorkerCommandHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }
    public async Task Handle(UpdatePermissionGroupsByWorkerCommand request, CancellationToken cancellationToken)
    {
        Worker worker = await _spaceDbContext.Workers
            .Include(w => w.PermissionGroups)
            .FirstOrDefaultAsync(w => w.Id == request.WorkerId, cancellationToken)
            ?? throw new NotFoundException(nameof(Worker), request.WorkerId);

        worker.PermissionGroups.Clear();
        List<PermissionGroup> permissionGroups = await _spaceDbContext.PermissionGroups
            .Where(pg => request.PermissionGroups.Select(pg => pg.Id).Contains(pg.Id))
            .ToListAsync(cancellationToken);
        if (permissionGroups.Count != request.PermissionGroups.Count())
            throw new NotFoundException(nameof(PermissionGroup));

        worker.PermissionGroups = permissionGroups;
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}