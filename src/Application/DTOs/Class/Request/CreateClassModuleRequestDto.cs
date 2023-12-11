namespace Space.Application.DTOs;

public class CreateClassModuleRequestDto
{
    public int ModuleId { get; set; }
    public int WorkerId { get; set; }
    public int RoleId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}
