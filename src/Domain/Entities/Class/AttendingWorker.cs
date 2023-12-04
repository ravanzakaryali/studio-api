namespace Space.Domain.Entities;

public class AttendingWorker : BaseAuditableEntity
{
    public Guid WorkerId { get; set; }
    public Worker Worker { get; set; } = null!;
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public Guid ClassSessionId { get; set; }
    public ClassSession ClassSession { get; set; } = null!;
}
