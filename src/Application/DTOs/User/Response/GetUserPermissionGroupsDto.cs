namespace Space.Application.DTOs;

public class GetUserPermissionGroupResponseDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public string? AvatarColor { get; set; }
    public List<string> PermissionGroups { get; set; } = new List<string>();
}