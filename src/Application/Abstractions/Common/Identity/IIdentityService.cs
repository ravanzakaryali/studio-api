﻿namespace Space.Application.Abstractions;

public interface IIdentityService
{
    //Login
    //Register
    Task<LoginResponseDto> LoginAsync(User user, string password);
    Task<User> RegisterAsync(RegisterDto register);
    Task<bool> RecaptchaVerifyAsync(string token);
}
