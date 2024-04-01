namespace Space.Domain.Entities;
public class PermissionGroup : BaseAuditableEntity
{
    public PermissionGroup()
    {
        PermissionGroupPermissionLevelAppModules = new HashSet<PermissionGroupPermissionLevelAppModule>();
        Workers = new HashSet<Worker>();
    }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ICollection<Worker> Workers { get; set; }
    public ICollection<PermissionGroupPermissionLevelAppModule> PermissionGroupPermissionLevelAppModules { get; set; }
}
