namespace Space.Domain.Entities;

public class Attendance : BaseAuditableEntity
{
    public string? Note { get; set; }
    public int ClassTimeSheetsId { get; set; }
    public ClassTimeSheet ClassTimeSheets { get; set; } = null!;
    public int StudyId { get; set; }
    public Study Student { get; set; } = null!;
    public int TotalAttendanceHours { get; set; }
    public StudentStatus? Status { get; set; }

}
