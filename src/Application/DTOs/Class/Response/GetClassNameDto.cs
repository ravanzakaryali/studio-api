namespace Space.Application.DTOs;

public class GetClassNameDto
{
    public string Name { get; set; } = null!;
    public GetProgramResponseDto Program { get; set; } = null!;
    public GetSessionResponseDto Session { get; set; } = null!;
    public DateOnly EndDate { get; set; }
}