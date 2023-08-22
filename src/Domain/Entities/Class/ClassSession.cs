namespace Space.Domain.Entities;

public class ClassSession : BaseAuditableEntity
{
    public ClassSession()
    {
        Attendances = new HashSet<Attendance>();
    }
    public DateTime Date { get; set;}
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int TotalHour { get; set; }
    public ClassSessionStatus? Status { get; set; }
    public string? Note { get; set; }
    public ClassSessionCategory? Category { get; set; }
    public Guid? WorkerId { get; set; }
    public Worker? Worker { get; set; }
    public Guid? RoomId { get; set; }
    public Room? Room { get; set; }
    public Guid? ModuleId { get; set; }
    public Module? Module { get; set; }
    public Guid ClassId { get; set; }
    public Class Class { get; set; } = null!;
    public ICollection<Attendance> Attendances { get; set; }
}
