using Space.Application.DTOs.Auth.Request;

namespace Space.Application.Abstractions;

public interface IIdentityService
{
    //Login
    //Register
    Task<LoginResponseDto> LoginAsync(string email,string password);
    Task<User> RegisterAsync(RegisterDto register);
    Task<bool> RecaptchaVerifyAsync(string token);
}
