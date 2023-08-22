namespace Space.Application.Abstractions;

public interface IRoleService
{
    //Add permission
    //Remove permission
    //Add role
    Task<Role> AddRoleAsync(string roleName);
    Task AddUserRoleAsync(string roleId, string userId);
    Task<IList<string>> GetRolesByUser(User user);
}
