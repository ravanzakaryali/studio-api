namespace Space.Application.DTOs;
public class UpdateClassModuleRequestDto
{
    public IEnumerable<CreateClassModuleRequestDto> Modules { get; set; } = null!;
    public IEnumerable<CreateClassExtraModuleRequestDto>? ExtraModules { get; set; }
    public IEnumerable<CreateClassNewExtraModuleRequestDto>? NewExtraModules { get; set; }
}