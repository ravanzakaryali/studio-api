namespace Space.Application.DTOs;

public class GetFincanceReportGetDto
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Fincode { get; set; } = null!;
    public string ProgramName { get; set; } = null!;
    public string ProjectName { get; set; } = null!;
    public int TotalHours { get; set; }
    public int TotalMinutes { get; set; }
}