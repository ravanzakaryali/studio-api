namespace Space.Application.DTOs;

public class ImportWorkerDto
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? Role { get; set; }
    public bool IsActive { get; set; }
    public string Email { get; set; } = null!;
}
