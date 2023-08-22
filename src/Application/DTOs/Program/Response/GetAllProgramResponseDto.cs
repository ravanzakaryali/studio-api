namespace Space.Application.DTOs;

public class GetAllProgramResponseDto
{
    public Guid Id { get; set; }
    public int TotalHours { get; set; }
    public string Name { get; set; } = null!;
    public IEnumerable<GetModuleDto>? Modules { get; set; }
}
