using Microsoft.AspNetCore.Identity;
using Space.Application.DTOs;
using Space.Application.Exceptions;
using System.Threading;

namespace Space.Infrastructure.Services;

public class UserService : IUserService
{
    readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<User> FindById(Guid id)
         => await _userManager.FindByIdAsync(id.ToString()) ??
            throw new NotFoundException("User", id);
    public async Task<User> FindByEmailAsync(string email)
        => await _userManager.FindByEmailAsync(email) ??
            throw new NotFoundException("User", email);
    public async Task<User> FindByNameAsync(string username)
       => await _userManager.FindByNameAsync(username) ??
           throw new NotFoundException("User", username);

    public async Task PasswordAssignAsync(Guid id, string password)
    {
        User user = await _userManager.FindByIdAsync(id.ToString());
        IdentityResult results = new();
        if (!await _userManager.HasPasswordAsync(user))
        {
            results = await _userManager.AddPasswordAsync(user, password);
        }
        else
        {
            results = await _userManager.RemovePasswordAsync(user);
            if (!results.Succeeded)
            {
                //Todo: Exception
                throw new Exception(results.Errors.FirstOrDefault()?.Description);
            }
            results = await _userManager.AddPasswordAsync(user, password);
        }
        if (!results.Succeeded) throw new Exception(results.Errors.FirstOrDefault()?.Description);
    }
    public async Task UpdateRefreshToken(string refreshToken, Token token, int addMinute = 15)
    {
        User? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpires = token.Expires.AddMinutes(addMinute);
    }
    public async Task CreateWorkerAsync(Worker worker)
    {
        IdentityResult result = await _userManager.CreateAsync(worker);
        if (!result.Succeeded) throw new Exception("Create worker exception");
    }
}
