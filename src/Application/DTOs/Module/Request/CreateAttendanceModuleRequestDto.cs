namespace Space.Application.DTOs;

public class CreateAttendanceModuleRequestDto
{
    public Guid ModuleId { get; set; }
    public int TotalHours { get; set; }
}
