namespace Space.Application.DTOs;

public class GetWorkersByClassResponseDto
{
    public int WorkerId { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public int TotalLessonHours { get; set; }
    public int CurrentLessonHours { get; set; }
    public string? RoleName { get; set; }
    public AttendanceStatus? AttendanceStatus { get; set; }
    public int? RoleId { get; set; }
    public int? TotalHours { get; set; }
    public int? TotalMinutes { get; set; }
}
