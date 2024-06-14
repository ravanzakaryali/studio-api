namespace Space.Application.DTOs;

public class CreatePermissionLevelDto
{
    public string Name { get; set; } = null!;
    public ICollection<PermissionAccessLevelDto> Accesses { get; set; } = null!;
}