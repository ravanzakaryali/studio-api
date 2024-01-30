using System;
namespace Space.Application.DTOs;


public class GetWorkerGeneralReportResponseDto
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? EMail { get; set; }
    public double AttendancePercent { get; set; }
    public int TotalHours { get; set; }
    public int CanceledHours { get; set; }
    public int CompletedHours { get; set; }
    public int TotalClasses { get; set; }
    public List<GetWorkerClassesForGeneralReportDto> CompletedClasses { get; set; } = null!;
    public List<GetWorkerClassesForGeneralReportDto> UnCompletedClasses { get; set; } = null!;

}


public class GetWorkerClassesForGeneralReportDto
{
    public string? ClassName { get; set; }
    public int ClassId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

}


