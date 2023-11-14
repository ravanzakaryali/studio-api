namespace Space.Domain.Entities;

public class SessionDetail : BaseAuditableEntity
{
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set;}
    public DayOfWeek DayOfWeek { get; set; }
    public int TotalHours { get; set; }
    public Guid SessionId { get; set; }
    public Session Session { get; set; } = null!;
    public int? KometaId { get; set; }
}
