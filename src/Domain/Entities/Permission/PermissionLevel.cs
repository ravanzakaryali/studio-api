namespace Space.Domain.Entities;
public class PermissionLevel : BaseAuditableEntity
{
    public PermissionLevel()
    {
        PermissionAccesses = new HashSet<PermissionAccess>();
        ApplicationModules = new HashSet<ApplicationModule>();
    }
    public string Name { get; set; } = null!;
    public ICollection<PermissionAccess> PermissionAccesses { get; set; }
    public ICollection<ApplicationModule> ApplicationModules { get; set; }
}