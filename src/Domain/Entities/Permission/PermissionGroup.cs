namespace Domain.Entities.Permission;
public class PermissionGroup : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}
