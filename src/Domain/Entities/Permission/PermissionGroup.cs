namespace Space.Domain.Entities;
public class PermissionGroup : BaseAuditableEntity
{
    public PermissionGroup()
    {
        Workers = new HashSet<Worker>();
    }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ICollection<Worker> Workers { get; set; }
}
