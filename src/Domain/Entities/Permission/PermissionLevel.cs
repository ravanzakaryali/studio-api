namespace Space.Domain.Entities;
public class PermissionLevel : BaseAuditableEntity
{
    public PermissionLevel()
    {
        PermissionGroupPermissionLevelAppModules = new HashSet<PermissionGroupPermissionLevelAppModule>();
        PermissionAccesses = new HashSet<PermissionAccess>();
        WorkerPermissionLevelAppModules = new HashSet<WorkerPermissionLevelAppModule>();
    }
    public string Name { get; set; } = null!;
    public ICollection<PermissionAccess> PermissionAccesses { get; set; }
    public ICollection<PermissionGroupPermissionLevelAppModule> PermissionGroupPermissionLevelAppModules { get; set; }
    public ICollection<WorkerPermissionLevelAppModule> WorkerPermissionLevelAppModules { get; set; }
}