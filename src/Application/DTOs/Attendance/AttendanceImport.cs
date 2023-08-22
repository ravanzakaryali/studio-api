namespace Space.Application.DTOs;

public class AttendanceImport
{
    public string Name { get; set; }
    public List<StudyDto> Studies { get; set; }
}
public class StudyDto
{
    public string FullName { get; set; }
    public string Status { get; set; }
    public List<AttendanceDto> Attendances { get; set; }
}
public class AttendanceDto
{
    public string Value { get; set; }
    public DateTime Date { get; set; }
}

