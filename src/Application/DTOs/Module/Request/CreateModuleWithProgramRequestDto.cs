namespace Space.Application.DTOs;

public class CreateModuleWithProgramRequestDto
{
    public int ProgramId { get; set; }
    public IEnumerable<ModuleDto> Modules { get; set; } = null!;
}
