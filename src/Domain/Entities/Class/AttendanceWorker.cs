namespace Space.Domain.Entities;

public class AttendanceWorker : BaseAuditableEntity
{
    public string? Note { get; set; }
    public int WorkerId { get; set; }
    public Worker Worker { get; set; } = null!;
    public int TotalHours { get; set; }
    public int TotalMinutes { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public int? RoleId { get; set; }
    public Role? Role { get; set; }
    public int ClassTimeSheetId { get; set; }
    public ClassTimeSheet ClassTimeSheet { get; set; } = null!;
}
