namespace Space.Application.DTOs;

public class RegisterDto
{
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Password { get; set; } = null!;
}
