using System.Text.Json.Serialization;

namespace Space.Application.DTOs;

public class SubModuleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public double Hours { get; set; }
    public string? Version { get; set; }
    public int? TopModuleId { get; set; }
    public bool IsSurvey { get; set; }
}
public class SubModuleDtoWithWorker : SubModuleDto
{
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    [JsonPropertyOrder(6)]
    public ICollection<GetWorkerForClassDto>? Workers { get; set; }
}
