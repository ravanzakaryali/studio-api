namespace Space.Domain.Entities;
public class PermissionGroup : BaseAuditableEntity
{
    public PermissionGroup()
    {
        Workers = new HashSet<Worker>();
        ApplicationModules = new HashSet<ApplicationModule>();
    }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ICollection<Worker> Workers { get; set; }
    public ICollection<ApplicationModule> ApplicationModules { get; set; }
}
