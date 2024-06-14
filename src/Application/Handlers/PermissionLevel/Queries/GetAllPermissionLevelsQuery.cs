namespace Space.Application.Handlers.Queries;

public class GetAllPermissionLevelsQuery : IRequest<IEnumerable<GetAllPermissionLevelDto>>
{

}
internal class GetAllPermissionLevelsQueryHandler : IRequestHandler<GetAllPermissionLevelsQuery, IEnumerable<GetAllPermissionLevelDto>>
{
    readonly ISpaceDbContext _spaceDbContext;
    public GetAllPermissionLevelsQueryHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetAllPermissionLevelDto>> Handle(GetAllPermissionLevelsQuery request, CancellationToken cancellationToken)
    {
        return await _spaceDbContext.PermissionLevels
            .Select(c => new GetAllPermissionLevelDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToListAsync(cancellationToken);
    }
}