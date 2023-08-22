namespace Space.Application.DTOs;

public class UpdateClassSessionRequestDto
{
    public ClassSessionCategory Category { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set;}
    public DateTime ClassSessionDate { get; set; }
}
