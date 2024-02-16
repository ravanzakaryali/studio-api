namespace Domain.Entities.Permission;
public class PermissionGroupUser : BaseAuditableEntity
{
    public int UserId { get; set; }
    public Worker User { get; set; } = null!;
    public int PermissionGroupId { get; set; }
    public PermissionGroup PermissionGroup { get; set; } = null!;
}