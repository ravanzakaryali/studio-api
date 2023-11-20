namespace Space.Application.DTOs;

public class AttendanceImport
{
    public string Name { get; set; } = null!;
    public List<StudyDto> Studies { get; set; } = null!;
}
public class StudyDto
{
    public string? FullName { get; set; }
    public string Status { get; set; } = null!;
    public List<AttendanceDto> Attendances { get; set; } = null!;
}
public class AttendanceDto
{
    public string Value { get; set; } = null!;
    public DateTime Date { get; set; }
}

