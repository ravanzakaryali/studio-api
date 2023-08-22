namespace Space.Application.DTOs;

public class CreateClassModuleRequestDto
{
    public Guid ModuleId { get; set; }
    public Guid WorkerId { get; set; }
    public Guid RoleId { get; set; }
}
