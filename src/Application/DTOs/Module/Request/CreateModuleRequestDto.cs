namespace Space.Application.DTOs;

public class CreateModuleRequestDto
{
    public Guid ProgramId { get; set; }
    public IEnumerable<ModuleDto> Modules { get; set; } = null!;
}
