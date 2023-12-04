namespace Space.Domain.Entities;

public class ClassSession : BaseAuditableEntity
{
    public Guid ClassId { get; set; }
    public Class Class { get; set; } = null!;
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int TotalHours { get; set; }
    public ClassSessionStatus Status { get; set; }
    public ClassSessionCategory Category { get; set; }
    public Guid? RoomId { get; set; }
    public Room? Room { get; set; }
    public bool IsHoliday { get; set; }
    public Guid? ClassTimeSheetId { get; set; }
    public ClassTimeSheet? ClassTimeSheet { get; set; }
}
