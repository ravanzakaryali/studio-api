namespace Space.Application.DTOs;

public class GetAttendanceRateByClassDto
{
    public int TotalStudentsCount { get; set; }
    public int AttendingStudentsCount { get; set; }
    public ClassSessionStatus Status { get; set; }
    public DateOnly Date { get; set; }
}
