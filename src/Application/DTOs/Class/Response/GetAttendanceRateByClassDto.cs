namespace Space.Application.DTOs;

public class GetAttendanceRateByClassDto
{
    public int? TotalStudentsCount { get; set; }
    public int? AttendingStudentsCount { get; set; }
    public ClassSessionStatus? Status { get; set; }
    public DateOnly Date { get; set; }
}

public class ClassAllSessionDto
{
    public DateOnly Date { get; set; }
    public int TotalHours { get; set; }
    public int ModuleId { get; set; }
}