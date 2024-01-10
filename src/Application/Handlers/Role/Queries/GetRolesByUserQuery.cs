using Microsoft.AspNetCore.Identity;

namespace Space.Application.Handlers;

public record GetRolesByUserQuery(int Id) : IRequest<IEnumerable<GetRoleDto>>;


internal class GetRolesByUserQueryHandler : IRequestHandler<GetRolesByUserQuery, IEnumerable<GetRoleDto>>
{
    readonly UserManager<User> _userManager;
    readonly RoleManager<Role> _roleManager;

    public GetRolesByUserQueryHandler(UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IEnumerable<GetRoleDto>> Handle(GetRolesByUserQuery request, CancellationToken cancellationToken)
    {
        User user = await _userManager.FindByIdAsync(request.Id.ToString())
            ?? throw new NotFoundException(nameof(User), request.Id);

        IList<string> names = await _userManager.GetRolesAsync(user);
        return await _roleManager.Roles.Where(r => names.ToList().Contains(r.Name)).Select(c => new GetRoleDto()
        {
            Name = c.Name,
            Id = c.Id,
        }).ToListAsync();


    }
}
