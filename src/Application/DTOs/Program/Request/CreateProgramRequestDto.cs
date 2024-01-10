namespace Space.Application.DTOs.Program.Request;

public class CreateProgramRequestDto
{
    public string Name { get; set; } = null!;
    public int TotalHours { get; set; }
}
