namespace Space.Application.DTOs;

public class GetClassNameDto
{
    public string Name { get; set; } = null!;
    public GetProgramResponseDto Program { get; set; } = null!;
    public GetSessionResponseDto Session { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}