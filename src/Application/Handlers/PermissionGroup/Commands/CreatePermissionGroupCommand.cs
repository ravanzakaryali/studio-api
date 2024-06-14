namespace Space.Application.Handlers;
public class CreatePermissionGroupCommand : IRequest
{
    public CreatePermissionGroupCommand()
    {
        WorkersId = new List<int>();
    }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ICollection<int> WorkersId { get; set; }
}
public class CreatePermissionGroupCommandHandler : IRequestHandler<CreatePermissionGroupCommand>
{
    readonly ISpaceDbContext _spaceDbContext;

    public CreatePermissionGroupCommandHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }
    public async Task Handle(CreatePermissionGroupCommand request, CancellationToken cancellationToken)
    {
        PermissionGroup permissionGroup = new()
        {
            Name = request.Name,
            Description = request.Description,
        };
        List<Worker> workers = await _spaceDbContext.Workers.Where(w => request.WorkersId.Contains(w.Id)).ToListAsync(cancellationToken);
        permissionGroup.Workers = workers;

        await _spaceDbContext.PermissionGroups.AddAsync(permissionGroup, cancellationToken);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}