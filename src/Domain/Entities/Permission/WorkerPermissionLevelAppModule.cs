namespace Space.Domain.Entities;

public class WorkerPermissionLevelAppModule : BaseEntity
{
    public int ApplicationModuleId { get; set; }
    public ApplicationModule ApplicationModule { get; set; } = null!;
    public int WorkerId { get; set; }
    public Worker Worker { get; set; } = null!;
    public int PermissionLevelId { get; set; }
    public PermissionLevel PermissionLevel { get; set; } = null!;
}