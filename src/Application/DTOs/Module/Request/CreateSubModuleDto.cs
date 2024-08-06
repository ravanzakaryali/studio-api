namespace Space.Application.DTOs;

public class CreateSubModuleDto
{
    public string Name { get; set; } = null!;
    public double Hours { get; set; }
    public string Version { get; set; } = null!;
    public bool IsAnket { get; set; }
}
