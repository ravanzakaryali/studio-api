namespace Space.Application.DTOs;

public class CreateClassModuleSessionRequestDto
{
    public IEnumerable<CreateClassModuleRequestDto> Modules { get; set; } = null!;
    public IEnumerable<CreateClassSessionDto> Sessions { get; set; } = null!;
}
