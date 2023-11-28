using System;
namespace Space.Application.DTOs;

public class GetStudentAttendancesByClassResponseDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; } = null!;
    public string? Surname { get; set; } = null!;
    public string? FatherName { get; set; }
    public string? Phone { get; set; } = null!;
    public string? Email { get; set; } = null!;
    public double AttendancePercent { get; set; }
    public List<AttendancesDto> Attendances { get; set; } = null!;
}

public class AttendancesDto
{
    public DateTime Date { get; set; }
    public int? AttendanceHours { get; set; }
    public string? Note { get; set; }
    public int LessonHours { get; set; }
    public ClassSessionCategory? Category { get; set; }
}


