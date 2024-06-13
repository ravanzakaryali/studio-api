namespace Space.Application.DTOs;



public class UpdatePermissionAppModuleDto
{
    public int Id { get; set; }
    public ICollection<PermissionAccessLevelDto> Accesses { get; set; } = null!;
}
