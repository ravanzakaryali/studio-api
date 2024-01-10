namespace Space.Application.DTOs;

public class GetUnMarkedAttendancesByProgramsDto
{
    public GetProgramResponseDto Program { get; set; } = null!;
    public double TotalAttendancePercentage { get; set; }
    public int UnMarkedAttendancesCount { get; set; }
    public int TotalUnMarkedAttendancesCount { get; set; }
}
