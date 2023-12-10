namespace Space.Domain.Entities;

public class AttendanceWorker : BaseAuditableEntity
{
    public string? Note { get; set; }
    public Guid WorkerId { get; set; }
    public Worker Worker { get; set; } = null!;
    public int TotalHours { get; set; }
    public int TotalMinutes { get; set; }
    public AttendanceStatus AttendanceStatus { get; set; }
    public Guid? RoleId { get; set; }
    public Role? Role { get; set; }
    public Guid ClassTimeSheetId { get; set; }
    public ClassTimeSheet ClassTimeSheet { get; set; } = null!;
}
