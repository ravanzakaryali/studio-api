namespace Space.Application.DTOs;

public class GetUnmarkedAttedanceClassesByProgramResponseDto
{
    public GetClassDto Class { get; set; } = null!;
    public int StudentsCount { get; set; }
    public double AttendancePercentage { get; set; }
    public int UnMarkDays { get; set; }
    public DateOnly LastDate { get; set; }
}
