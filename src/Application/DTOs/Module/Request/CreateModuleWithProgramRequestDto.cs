namespace Space.Application.DTOs;

public class CreateModuleWithProgramRequestDto
{
    public Guid ProgramId { get; set; }
    public IEnumerable<ModuleDto> Modules { get; set; } = null!;
}
