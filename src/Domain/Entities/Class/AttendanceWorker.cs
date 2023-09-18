namespace Space.Domain.Entities;

public class AttendanceWorker : BaseAuditableEntity
{
    public string? Note { get; set; }
    public Guid ClassSessionId { get; set; }
    public ClassSession ClassSession { get; set; } = null!;
    public Guid WorkerId { get; set; }
    public Worker Worker { get; set; } = null!;
    public int TotalAttendanceHours { get; set; }
}
