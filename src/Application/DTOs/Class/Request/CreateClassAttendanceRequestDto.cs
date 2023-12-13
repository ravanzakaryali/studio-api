namespace Space.Application.DTOs;

public class CreateClassAttendanceRequestDto
{
    public DateOnly Date { get; set; }
    public ICollection<UpdateAttendanceCategorySessionDto> Sessions { get; set; } = null!;
    public ICollection<CreateAttendanceModuleRequestDto>? HeldModules { get; set; }
}
