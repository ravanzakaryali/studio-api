namespace Space.Application.DTOs;

public class GetHeldModulesDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Version { get; set; }
    public int TotalHours { get; set; }
}
