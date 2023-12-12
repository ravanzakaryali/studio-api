namespace Space.Application.DTOs;

public class GetUserResponseDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public List<string> RoleName { get; set; } = new List<string>();
}
