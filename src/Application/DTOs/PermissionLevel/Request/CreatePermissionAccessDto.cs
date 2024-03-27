namespace Space.Application.DTOs;

public class CreatePermissionAccessDto
{
    public string Name { get; set; } = null!;
    public ICollection<CreatePermissionAccessLevelDto> Accesses { get; set; } = null!;
}