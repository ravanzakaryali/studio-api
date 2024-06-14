namespace Space.Application.DTOs;

public class GetAllPermissionGroupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}