namespace Space.Domain.Entities;

public class SessionDetail : BaseAuditableEntity
{
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public ClassSessionCategory Category { get; set; }
    public int TotalHours { get; set; }
    public int SessionId { get; set; }
    public Session Session { get; set; } = null!;
    public int? KometaId { get; set; }
}
