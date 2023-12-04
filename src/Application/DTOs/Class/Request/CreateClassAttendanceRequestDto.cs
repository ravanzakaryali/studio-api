namespace Space.Application.DTOs;

public class CreateClassAttendanceRequestDto
{
    public Guid ModuleId { get; set; }
    public DateOnly Date { get; set; }
    public ICollection<UpdateAttendanceCategorySessionDto> Sessions { get; set; } = null!;
}
