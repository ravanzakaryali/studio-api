namespace Space.Application.DTOs;


public class GetAllPermissionLevelDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}
public class GetAllPermissionLevelWithAccessesDto : GetAllPermissionLevelDto
{
    public ICollection<GetPermissionAccessDto> PermissionAccesses { get; set; } = null!;
}
