namespace Space.Domain.Entities;
public class PermissionLevel : BaseAuditableEntity
{
    public PermissionLevel()
    {
        PermissionGroupAppModulePermissionLevels = new HashSet<PermissionGroupAppModulePermissionLevel>();
        PermissionAccesses = new HashSet<PermissionAccess>();
        PermissionGroupWorkerPermissionLevels = new HashSet<PermissionGroupWorkerPermissionLevel>();
    }
    public string Name { get; set; } = null!;
    public ICollection<PermissionAccess> PermissionAccesses { get; set; }
    public ICollection<PermissionGroupAppModulePermissionLevel> PermissionGroupAppModulePermissionLevels { get; set; }
    public ICollection<PermissionGroupWorkerPermissionLevel> PermissionGroupWorkerPermissionLevels { get; set; }
}