namespace Space.Application.DTOs;
public class SetAccessToPermissionGroupDto
{
    public SetAccessToPermissionGroupDto()
    {
        PermissionLevels = new List<PermissionGroupSetPermissionLevelDto>();
    }
    public ICollection<PermissionGroupSetPermissionLevelDto> PermissionLevels { get; set; }
}
public class PermissionGroupSetPermissionLevelDto
{
    public int PermissionLevelId { get; set; }
    public bool IsAccess { get; set; }
}
