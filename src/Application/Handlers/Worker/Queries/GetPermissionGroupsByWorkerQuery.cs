namespace Space.Application.Handlers;

public class GetPermissionGroupsByWorkerQuery : IRequest<IEnumerable<GetAllPermissionGroupDto>>
{
    public GetPermissionGroupsByWorkerQuery(int workerId)
    {
        WorkerId = workerId;
    }
    public int WorkerId { get; }
}
internal class GetPermissionGroupsByWorkerQueryHandler : IRequestHandler<GetPermissionGroupsByWorkerQuery, IEnumerable<GetAllPermissionGroupDto>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetPermissionGroupsByWorkerQueryHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }
    public async Task<IEnumerable<GetAllPermissionGroupDto>> Handle(GetPermissionGroupsByWorkerQuery request, CancellationToken cancellationToken)
    {
        Worker? worker = await _spaceDbContext.Workers
            .Include(w => w.PermissionGroups)
            .FirstOrDefaultAsync(w => w.Id == request.WorkerId, cancellationToken)
            ?? throw new NotFoundException(nameof(Worker), request.WorkerId);

        return worker.PermissionGroups.Select(pg => new GetAllPermissionGroupDto()
        {
            Id = pg.Id,
            Name = pg.Name,
            Description = pg.Description,
        });
    }
}