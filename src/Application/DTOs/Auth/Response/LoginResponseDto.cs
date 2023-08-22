namespace Space.Application.DTOs;

public class LoginResponseDto
{
    public User User { get; set; } = null!;
    public IList<string> Roles { get; set; } = null!;
}
