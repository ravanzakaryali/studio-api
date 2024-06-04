namespace Space.Application.Handlers;
public class AddWorkerToPermissionGroupCommand : IRequest
{
    public AddWorkerToPermissionGroupCommand()
    {
        WorkerIds = new HashSet<int>();
    }
    public int PermissionGroupId { get; set; }
    public IEnumerable<int> WorkerIds { get; set; }
}
public class AddWorkerToPermissionGroupCommandHandler : IRequestHandler<AddWorkerToPermissionGroupCommand>
{
    readonly ISpaceDbContext _spaceDbContext;

    public AddWorkerToPermissionGroupCommandHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }
    public async Task Handle(AddWorkerToPermissionGroupCommand request, CancellationToken cancellationToken)
    {
        PermissionGroup? permissionGroup = await _spaceDbContext.PermissionGroups
                                                            .Include(pg => pg.Workers)
                                                            .FirstOrDefaultAsync(pg => pg.Id == request.PermissionGroupId, cancellationToken)
                                                                ?? throw new NotFoundException(nameof(PermissionGroup), request.PermissionGroupId);


        List<Worker> workers = await _spaceDbContext.Workers
                                            .Where(w => request.WorkerIds.Contains(w.Id))
                                            .ToListAsync(cancellationToken);

        if (workers.Count != request.WorkerIds.Count())
            throw new NotFoundException(nameof(Worker), request.WorkerIds.Except(workers.Select(c => c.Id)).FirstOrDefault());

        workers.ForEach(worker =>
        {
            permissionGroup.Workers.Add(worker);
        });

        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}