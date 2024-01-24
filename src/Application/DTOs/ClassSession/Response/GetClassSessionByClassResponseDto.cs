namespace Space.Application.DTOs;

public class GetClassSessionByClassResponseDto
{
    public string ClassName { get; set; } = null!;
    public ClassSessionCategory? Category { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set;}
    public int TotalHours { get; set; }
}

public class GetClassSessionByClassByDayResponseDto
{
    public string ClassName { get; set; } = null!;
    public ClassSessionCategory? Category { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set;}
    public int Hours { get; set; }
}
