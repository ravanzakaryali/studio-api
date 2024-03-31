namespace Space.Application.Handlers.Queries;

public class GetAllPermissionLevelsWithAccessesQuery : IRequest<IEnumerable<GetAllPermissionLevelDto>>
{

}
internal class GetAllPermissionLevelsWithAccessesQueryHandler : IRequestHandler<GetAllPermissionLevelsWithAccessesQuery, IEnumerable<GetAllPermissionLevelDto>>
{
    readonly ISpaceDbContext _spaceDbContext;
    public GetAllPermissionLevelsWithAccessesQueryHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetAllPermissionLevelDto>> Handle(GetAllPermissionLevelsWithAccessesQuery request, CancellationToken cancellationToken)
    {
        List<GetAllPermissionLevelWithAccessesDto> permissionLevels = await _spaceDbContext.PermissionLevels
             .Include(c => c.PermissionAccesses)
             .Select(c => new GetAllPermissionLevelWithAccessesDto
             {
                 Id = c.Id,
                 Name = c.Name,
                 PermissionAccesses = c.PermissionAccesses.Select(p => new GetPermissionAccessDto
                 {
                     PermissionLevelId = p.Id,
                     Name = p.Name,
                     IsAccess = false
                 }).ToList()
             }).ToListAsync(cancellationToken);

        List<PermissionAccess> permissionAccesses = await _spaceDbContext.PermissionAccesses.ToListAsync(cancellationToken);
        foreach (GetAllPermissionLevelWithAccessesDto permissionLevel in permissionLevels)
        {
            permissionLevel.PermissionAccesses = permissionAccesses.Select(c => new GetPermissionAccessDto
            {
                PermissionLevelId = c.Id,
                Name = c.Name,
                IsAccess = permissionLevel.PermissionAccesses.Any(p => p.PermissionLevelId == c.Id)
            }).ToList();
        }
        return permissionLevels;
    }
}