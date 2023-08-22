namespace Space.Application.DTOs;

public class UpdatePasswordRequestDto
{
    public Guid Key { get; set; }
    public string Password { get; set; } = null!;
}
