namespace Space.Application.Handlers;

public class GetPermissionGroupsQuery : IRequest<IEnumerable<GetPermissionGroupDto>>
{

}
internal class GetPermissionGroupsQueryHandler : IRequestHandler<GetPermissionGroupsQuery, IEnumerable<GetPermissionGroupDto>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetPermissionGroupsQueryHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }
    public async Task<IEnumerable<GetPermissionGroupDto>> Handle(GetPermissionGroupsQuery request, CancellationToken cancellationToken)
    {
        return await _spaceDbContext.PermissionGroups
            .Include(pg => pg.Workers)
            .Select(pg => new GetPermissionGroupDto
            {
                Id = pg.Id,
                Name = pg.Name,
                UserCount = pg.Workers.Count,
                Description = pg.Description,
                Workers = pg.Workers.OrderByDescending(c => c.CreatedBy).Take(5).Select(w => new GetWorkerResponseDto
                {
                    Id = w.Id,
                    Name = w.Name!,
                    Surname = w.Surname!,
                    Email = w.Email,
                    AvatarColor = w.AvatarColor,
                }).ToList()
            }).ToListAsync(cancellationToken);
    }
}
