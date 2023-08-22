using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Space.Application.DTOs.Auth.Request;
using Space.Application.Exceptions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Space.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    readonly UserManager<User> _userManager;
    readonly IMapper _mapper;
    readonly IConfiguration _configuration;

    public IdentityService(UserManager<User> userManager, IMapper mapper, IConfiguration configuration)
    {
        _userManager = userManager;
        _mapper = mapper;
        _configuration = configuration;
    }
    public async Task<LoginResponseDto> LoginAsync(string email, string password)
    {
        User user = await _userManager.FindByEmailAsync(email) ?? 
            throw new InvalidCredentialsException();
        bool passwordCheck = await _userManager.CheckPasswordAsync(user, password);
        if (!passwordCheck) throw new InvalidCredentialsException();
        IList<string> roles = await _userManager.GetRolesAsync(user);
        return new LoginResponseDto { Roles = roles, User = user };
    }

    public async Task<bool> RecaptchaVerifyAsync(string token)
    {
        string googleRecaptchaVerifyApi = _configuration["ReCaptcha:VefiyAPIAddress"];
        string googleSecretKey = _configuration["ReCaptcha:SecretKey"];
        decimal scoreThreshold = decimal.Parse(_configuration["ReCaptcha:ScoreThreshold"]);
        using var client = new HttpClient();
        var response = await client.GetStringAsync($"{googleRecaptchaVerifyApi}?secret={googleSecretKey}&response={token}");
        var tokenResponse = JsonSerializer.Deserialize<RecaptchaTokenResponseDto>(response)!;
        if (!tokenResponse.success)
        {
            return false;
        }
        return true;
    }

    public async Task<User> RegisterAsync(RegisterDto register)
    {
        User userDb = await _userManager.FindByNameAsync(register.Email);
        if (userDb != null) throw new Exception("Already exception");
        User newUser = _mapper.Map<User>(register);
        IdentityResult result = await _userManager.CreateAsync(newUser, register.Password);
        if (!result.Succeeded) throw new Exception("Register exception");
        return newUser!;
    }

}
