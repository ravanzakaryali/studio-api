using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Identity;

namespace Space.Application.Handlers;

public record CreateRoleByUserCommand(int Id, IEnumerable<CreateRoleRequestDto> Roles) : IRequest;

internal class CreateRoleByUserCommandHandler : IRequestHandler<CreateRoleByUserCommand>
{
    readonly UserManager<User> _userManager;
    readonly ISpaceDbContext _spaceDbContext;
    readonly RoleManager<Role> _roleManager;

    public CreateRoleByUserCommandHandler(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        ISpaceDbContext spaceDbContext)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(CreateRoleByUserCommand request, CancellationToken cancellationToken)
    {
        User user = await _userManager.FindByIdAsync(request.Id.ToString())
            ?? throw new NotFoundException(nameof(User), request.Id);
        List<int> roles = request.Roles.Select(r => r.Id).ToList();
        IList<string> userRoles = await _userManager.GetRolesAsync(user);
        if (userRoles.Count != 0) await _userManager.RemoveFromRolesAsync(user, userRoles);
        List<string> rolesDb = await _roleManager.Roles.Where(r => roles.Contains(r.Id)).Select(c => c.Name).ToListAsync();
        user.SecurityStamp ??= Guid.NewGuid().ToString();
        await _userManager.AddToRolesAsync(user, rolesDb);
    }
}
