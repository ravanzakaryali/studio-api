namespace Space.Domain.Entities;

public class Attendance : BaseAuditableEntity
{
    public string? Note { get; set; }
    public Guid ClassSessionId { get; set; }
    public ClassTimeSheet ClassSession { get; set; } = null!;
    public Guid StudyId { get; set; }
    public Study Student { get; set; } = null!;
    public int TotalAttendanceHours { get; set; }
    public StudentStatus Status { get; set; }

}
