using Microsoft.AspNetCore.Identity;
using Space.Application.Exceptions;

namespace Space.Infrastructure.Services;

public class RoleService : IRoleService
{
    readonly RoleManager<Role> _roleManager;
    readonly UserManager<User> _userManager;

    public RoleService(RoleManager<Role> roleManager, UserManager<User> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<Role> AddRoleAsync(string roleName)
    {
        Role newRole = new()
        {
            Name = roleName,
        };
        IdentityResult result = await _roleManager.CreateAsync(newRole);
        if (!result.Succeeded) throw new Exception("Add role error"); //Todo: Identity exception
        return newRole;
    }

    public async Task AddUserRoleAsync(string roleId, string userId)
    {

        User userDb = await _userManager.FindByIdAsync(userId) 
            ?? throw new NotFoundException("User",userId);
        Role roleDb = await _roleManager.FindByIdAsync(roleId)
            ?? throw new NotFoundException("Role",roleId);
        IdentityResult result = await _userManager.AddToRoleAsync(userDb, roleDb.Name);
        //Todo: Identity exception
        if (!result.Succeeded) throw new Exception("Add role exception");
    }

    public async Task<IList<string>> GetRolesByUser(User user)
        => await _userManager.GetRolesAsync(user);
       
}
