namespace Space.Application.DTOs;

public class GetClassTimeSheetResponseDto
{
    public int ClassTimeSheetId { get; set; }
    public GetClassDto Class { get; set; } = null!;
    public TimeOnly StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public ClassSessionCategory Category { get; set; }
}