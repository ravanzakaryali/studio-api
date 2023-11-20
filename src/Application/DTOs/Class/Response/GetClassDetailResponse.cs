namespace Space.Application.DTOs;

public class GetClassDetailResponse
{
    public string Name { get; set; } = null!;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int TotalHours { get; set; }
    public GetProgramResponseDto Program { get; set; } = null!;
    public GetSessionResponseDto Session { get; set; } = null!;
    public double AttendanceRate { get; set; }
}
