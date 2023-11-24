namespace Space.Domain.Entities;

public class ClassTimeSheet : BaseAuditableEntity, ICloneable
{
    public ClassTimeSheet()
    {
        Attendances = new HashSet<Attendance>();
        AttendancesWorkers = new List<AttendanceWorker>();
        Modules = new HashSet<Module>();
    }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int TotalHours { get; set; }
    public ClassSessionStatus Status { get; set; }
    public string? Note { get; set; }
    public ClassSessionCategory? Category { get; set; }
    public Guid ClassId { get; set; }
    public Class Class { get; set; } = null!;
    public ICollection<Module> Modules { get; set; }
    public List<AttendanceWorker> AttendancesWorkers { get; set; }
    public ICollection<Attendance> Attendances { get; set; }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}
