namespace Domain.Entities.Permission;
public class PermissionAccess : BaseAuditableEntity
{
    public PermissionAccess()
    {
        PermissionLevels = new HashSet<PermissionLevel>();
    }
    public string Name { get; set; } = null!;
    public ICollection<PermissionLevel> PermissionLevels { get; set; }
}
