namespace Space.Application.Handlers.Queries;

public class GetPermissionAccessesQuery : IRequest<IEnumerable<GetPermissionLevelDto>>
{

}
internal class GetPermissionAccessesQueryHandler : IRequestHandler<GetPermissionAccessesQuery, IEnumerable<GetPermissionLevelDto>>
{
    readonly ISpaceDbContext _spaceDbContext;
    public GetPermissionAccessesQueryHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetPermissionLevelDto>> Handle(GetPermissionAccessesQuery request, CancellationToken cancellationToken)
    {
        List<GetPermissionLevelDto> permissionAccesses = await _spaceDbContext.PermissionAccesses
            .Select(a => new GetPermissionLevelDto()
            {
                Id = a.Id,
                Name = a.Name
            })
            .ToListAsync(cancellationToken: cancellationToken);
        
        return permissionAccesses;
    }
}