namespace Space.Domain.Entities;

public class PermissionGroupWorkerPermissionLevel : BaseEntity
{
    public int PermissionGroupId { get; set; }
    public PermissionGroup PermissionGroup { get; set; } = null!;
    public int WorkerId { get; set; }
    public Worker Worker { get; set; } = null!;
    public int PermissionLevelId { get; set; }
    public PermissionLevel PermissionLevel { get; set; } = null!;
}