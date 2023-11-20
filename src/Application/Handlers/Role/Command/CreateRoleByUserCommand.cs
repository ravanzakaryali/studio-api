using Microsoft.AspNetCore.Identity;

namespace Space.Application.Handlers;

public record CreateRoleByUserCommand(Guid Id, IEnumerable<CreateRoleRequestDto> Roles) : IRequest;

internal class CreateRoleByUserCommandHandler : IRequestHandler<CreateRoleByUserCommand>
{
    readonly UserManager<User> _userManager;
    readonly RoleManager<Role> _roleManager;

    public CreateRoleByUserCommandHandler(
        UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task Handle(CreateRoleByUserCommand request, CancellationToken cancellationToken)
    {
        User user = await _userManager.FindByIdAsync(request.Id.ToString())
            ?? throw new NotFoundException(nameof(User), request.Id);
        List<Guid> roles = request.Roles.Select(r => r.Id).ToList();
        IList<string> userRoles = await _userManager.GetRolesAsync(user);
        if (userRoles.Count != 0) await _userManager.RemoveFromRolesAsync(user, userRoles);
        List<string> rolesDb = await _roleManager.Roles.Where(r => roles.ToList().Contains(r.Id)).Select(c => c.Name).ToListAsync();
        user.SecurityStamp ??= Guid.NewGuid().ToString();
        await _userManager.AddToRolesAsync(user, rolesDb);
    }
}
