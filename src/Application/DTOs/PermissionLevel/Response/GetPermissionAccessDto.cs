namespace Space.Application.DTOs;
public class GetPermissionAccessDto
{
    public int Id { get; set; }
    public bool IsAccess { get; set; }
    public string Name { get; set; } = null!;
}