namespace Space.Application.Handlers.Queries;

public class GetAllPermissionLevelQuery : IRequest<IEnumerable<GetAllPermissionLevelDto>>
{

}
internal class GetAllPermissionLevelQueryHandler : IRequestHandler<GetAllPermissionLevelQuery, IEnumerable<GetAllPermissionLevelDto>>
{
    readonly ISpaceDbContext _spaceDbContext;
    public GetAllPermissionLevelQueryHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetAllPermissionLevelDto>> Handle(GetAllPermissionLevelQuery request, CancellationToken cancellationToken)
    {
        List<GetAllPermissionLevelDto> permissionLevels = await _spaceDbContext.PermissionLevels
             .Include(c => c.PermissionAccesses)
             .Select(c => new GetAllPermissionLevelDto
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
        foreach (GetAllPermissionLevelDto permissionLevel in permissionLevels)
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