namespace Space.Application.DTOs;

public class StartAttendanceRequestDto
{
    public int ClassId { get; set; }
    public ClassSessionCategory SessionCategory { get; set; }
    public ICollection<CreateAttendanceModuleRequestDto>? HeldModules { get; set; }
    public int WorkerId { get; set; }
}