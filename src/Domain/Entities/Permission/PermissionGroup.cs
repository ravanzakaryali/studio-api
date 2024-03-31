namespace Space.Domain.Entities;
public class PermissionGroup : BaseAuditableEntity
{
    public PermissionGroup()
    {
        PermissionGroupAppModulePermissionLevels = new HashSet<PermissionGroupAppModulePermissionLevel>();
        PermissionGroupWorkerPermissionLevels = new HashSet<PermissionGroupWorkerPermissionLevel>();
        Workers = new HashSet<Worker>();
    }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ICollection<Worker> Workers { get; set; }
    public ICollection<PermissionGroupAppModulePermissionLevel> PermissionGroupAppModulePermissionLevels { get; set; }
    public ICollection<PermissionGroupWorkerPermissionLevel> PermissionGroupWorkerPermissionLevels { get; set; }
}
