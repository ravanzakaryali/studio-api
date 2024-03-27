namespace Space.Application.DTOs;

public class CreatePermissionLevelDto
{
    public string Name { get; set; } = null!;
    public ICollection<CreatePermissionAccessLevelDto> Accesses { get; set; } = null!;
}