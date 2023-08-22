namespace Space.Application.DTOs;

public class GetSessionDetailDto
{
    public string ClassName { get; set; } = null!;
    public Guid Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public int TotalHours { get; set; }
}
