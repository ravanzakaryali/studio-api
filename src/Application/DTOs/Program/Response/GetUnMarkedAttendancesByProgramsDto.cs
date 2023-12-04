namespace Space.Application.DTOs;

public class GetUnMarkedAttendancesByProgramsDto
{
    public double TotalAttendance { get; set; }
    public int UnMarkedAttendancesCount { get; set; }
    public int TotalUnMarkedAttendancesCount { get; set; }
}
