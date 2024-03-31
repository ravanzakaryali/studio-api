namespace Space.Application.Handlers.Queries;

public class DeletePermissionLevelCommand : IRequest
{
    public int Id { get; set; }
}
internal class DeletePermissionLevelCommandHandler : IRequestHandler<DeletePermissionLevelCommand>
{

    readonly ISpaceDbContext _spaceDbContext;
    public DeletePermissionLevelCommandHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(DeletePermissionLevelCommand request, CancellationToken cancellationToken)
    {
        PermissionLevel? permissionLevel = await _spaceDbContext.PermissionLevels.Where(c => c.Id == request.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    ?? throw new NotFoundException(nameof(PermissionLevel), request.Id);
        _spaceDbContext.PermissionLevels.Remove(permissionLevel);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}