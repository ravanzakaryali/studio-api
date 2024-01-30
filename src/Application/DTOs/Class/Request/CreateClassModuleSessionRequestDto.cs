namespace Space.Application.DTOs;

public class CreateClassModuleSessionRequestDto
{
    public IEnumerable<CreateClassModuleRequestDto> Modules { get; set; } = null!;
    public int SessionId { get; set; }
}
