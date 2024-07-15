namespace Space.Application.DTOs;

public class GetWorkerResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? AvatarColor { get; set; }
}
