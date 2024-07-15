namespace Space.Application.Handlers.Queries;

public class CreatePermissionLevelCommand : IRequest
{
    public string Name { get; set; } = null!;
    public ICollection<PermissionAccessLevelDto> Accesses { get; set; } = null!;
}
internal class CreatePermissionLevelCommandHandler : IRequestHandler<CreatePermissionLevelCommand>
{

    readonly ISpaceDbContext _spaceDbContext;
    public CreatePermissionLevelCommandHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(CreatePermissionLevelCommand request, CancellationToken cancellationToken)
    {

        PermissionLevel? permissionLevelDb = await _spaceDbContext.PermissionLevels.Where(c => c.Name == request.Name).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (permissionLevelDb != null)
        {
            throw new AlreadyExistsException(nameof(PermissionLevel), request.Name);
        }
        //find the permission accesses
        List<PermissionAccess> permissionAccesses = await _spaceDbContext.PermissionAccesses.ToListAsync();

        PermissionLevel permissionLevel = new()
        {
            Name = request.Name,
        };
        foreach (var access in request.Accesses)
        {
            PermissionAccess? permissionAccess = permissionAccesses.Where(c => c.Id == access.Id).FirstOrDefault()
                    ?? throw new NotFoundException(nameof(PermissionAccess), access.Id);

            if (access.IsAccess)
            {
                permissionLevel.PermissionAccesses.Add(permissionAccess);
            }
        }
        await _spaceDbContext.PermissionLevels.AddAsync(permissionLevel, cancellationToken);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}