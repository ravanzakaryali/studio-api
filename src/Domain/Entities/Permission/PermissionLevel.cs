namespace Domain.Entities.Permission;
public class PermissionLevel : BaseAuditableEntity
{
    public PermissionLevel()
    {
        PermissionAccesses = new HashSet<PermissionAccess>();
    }
    public string Name { get; set; } = null!;
    public ICollection<PermissionAccess> PermissionAccesses { get; set; }
}