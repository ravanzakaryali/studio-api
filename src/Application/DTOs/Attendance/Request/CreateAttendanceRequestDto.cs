namespace Space.Application.DTOs;

public class CreateAttendanceRequestDto
{
    public int ClassTimeSheetsId { get; set; }
    public ICollection<UpdateAttendanceDto> Attendances { get; set; } = null!;
}