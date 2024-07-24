namespace Space.Application.DTOs;

public class GetFincanceReportGetDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? Role { get; set; }
    public string Fincode { get; set; } = null!;
    public string ProgramName { get; set; } = null!;
    public string GroupName { get; set; } = null!;
    public int TotalHours { get; set; }
    public int TotalMinutes { get; set; }
}