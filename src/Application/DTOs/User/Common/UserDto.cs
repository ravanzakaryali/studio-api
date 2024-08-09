namespace Space.Application.DTOs;
public class UserDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public WorkerType WorkerType { get; set; }
}