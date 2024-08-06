namespace Space.Application.DTOs;

public class GetModuleDto 
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public double Hours { get; set; }
    public string? Version { get; set; }
    public bool IsAnket { get; set; }
    public IEnumerable<SubModuleDto>? SubModules { get; set; } 
}
