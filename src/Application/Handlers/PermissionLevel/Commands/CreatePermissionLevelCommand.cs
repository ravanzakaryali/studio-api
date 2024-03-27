namespace Space.Application.Handlers.Queries;

public class CreatePermissionLevelCommand : IRequest<CreatePermissionLevelDto>
{
    public string Name { get; set; } = null!;
    public ICollection<CreatePermissionAccessLevelDto> Accesses { get; set; } = null!;
}
internal class CreatePermissionLevelCommandHandler : IRequestHandler<CreatePermissionLevelCommand, CreatePermissionLevelDto>
{

    readonly ISpaceDbContext _spaceDbContext;
    public CreatePermissionLevelCommandHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<CreatePermissionLevelDto> Handle(CreatePermissionLevelCommand request, CancellationToken cancellationToken)
    {

        PermissionLevel? permissionLevelDb = await _spaceDbContext.PermissionLevels.Where(c => c.Name == request.Name).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (permissionLevelDb != null)
        {
            throw new AlreadyExistsException(nameof(PermissionLevel), request.Name);
        }
        throw new NotImplementedException();
    }
}