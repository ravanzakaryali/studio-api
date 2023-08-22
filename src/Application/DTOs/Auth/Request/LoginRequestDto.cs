namespace Space.Application.DTOs.Auth.Request;

public class LoginRequestDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    //public string Token { get; set; } = null!;
}
