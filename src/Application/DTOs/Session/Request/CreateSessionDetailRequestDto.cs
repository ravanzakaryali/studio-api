namespace Space.Application.DTOs;

[Serializable]
public class CreateSessionDetailRequestDto
{
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set;}
    public DayOfWeek DayOfWeek { get; set; }
    public int TotalHours { get; set; }
}
