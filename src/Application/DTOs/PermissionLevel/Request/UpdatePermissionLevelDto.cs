namespace Space.Application.DTOs;

public class UpdatePermissionLevelDto
{
    public int Id { get; set; }
    public ICollection<PermissionAccessLevelDto> Accesses { get; set; } = null!;
}