namespace Space.Application.DTOs;

public class StartAttendanceRequestDto
{
    public int ClassId { get; set; }
    public ClassSessionCategory SessionCategory { get; set; }
    public ICollection<int>? HeldModulesIds { get; set; }
    public int WorkerId { get; set; }
}