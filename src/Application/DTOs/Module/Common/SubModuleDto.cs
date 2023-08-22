using System.Text.Json.Serialization;

namespace Space.Application.DTOs;

public class SubModuleDto 
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public double Hours { get; set; }
    public string? Version { get; set; }
    public Guid? TopModuleId { get; set; }
}
public class SubModuleDtoWithWorker : SubModuleDto
{
    [JsonPropertyOrder(6)]
    public ICollection<GetWorkerForClassDto>? Workers { get; set; }
}
