namespace Space.Application.DTOs;

public class CreateWorkerDto
{
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
}
