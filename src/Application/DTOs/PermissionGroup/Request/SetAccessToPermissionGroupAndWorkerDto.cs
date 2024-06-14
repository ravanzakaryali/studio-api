namespace Space.Application.DTOs;
public class SetAccessToPermissionGroupAndWorkerDto
{
    public SetAccessToPermissionGroupAndWorkerDto()
    {
        PermissionLevels = new List<PermissionGroupSetPermissionLevelDto>();
    }
    public int AppModuleId { get; set; }
    public ICollection<PermissionGroupSetPermissionLevelDto> PermissionLevels { get; set; }
}
public class PermissionGroupSetPermissionLevelDto
{
    public int PermissionLevelId { get; set; }
    public bool IsAccess { get; set; }
}
