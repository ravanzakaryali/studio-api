namespace Space.Application.DTOs;

public class GetExtraModuleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Version { get; set; }
    public double Hours { get; set; }
}