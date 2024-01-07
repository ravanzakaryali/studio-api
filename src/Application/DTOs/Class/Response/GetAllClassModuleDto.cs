namespace Space.Application.DTOs;
public class GetAllClassModuleDto
{
    public IEnumerable<GetClassExtraModuleResponseDto> ExtraModules { get; set; } = null!;
    public IEnumerable<GetClassModuleResponseDto> Modules { get; set; } = null!;
}