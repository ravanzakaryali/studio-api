namespace Space.Application.Handlers;

public class GetAllRolesQuery : IRequest<IEnumerable<GetRoleDto>>
{
}
internal class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, IEnumerable<GetRoleDto>>
{
    public readonly RoleManager<Role> _roleManager;

    public GetAllRolesQueryHandler(RoleManager<Role> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<IEnumerable<GetRoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        return await _roleManager.Roles.Select(r => new GetRoleDto()
        {
            Id = r.Id,
            Name = r.Name
        }).ToListAsync();

    }
}
