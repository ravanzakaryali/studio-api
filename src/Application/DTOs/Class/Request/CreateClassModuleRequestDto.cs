namespace Space.Application.DTOs;

public class CreateClassModuleRequestDto
{
    public Guid ModuleId { get; set; }
    public Guid WorkerId { get; set; }
    public Guid RoleId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
