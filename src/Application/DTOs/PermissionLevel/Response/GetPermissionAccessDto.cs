namespace Space.Application.DTOs;
public class GetPermissionAccessDto
{
    public int PermissionLevelId { get; set; }
    public bool IsAccess { get; set; }
    public string Name { get; set; } = null!;
}