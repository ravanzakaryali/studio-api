namespace Space.Application.DTOs;

public class GetClassResponseDto
{
    public int Id { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int ProgramId { get; set; }
    public int SessionId { get; set; }
    public int? RoomId { get; set; }
}
public class GetClassModuleResponseDto
{
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public double Hours { get; set; }
    public string? Version { get; set; }
    public List<SubModuleDtoWithWorker>? SubModules { get; set; }
    public ICollection<GetWorkerForClassDto>? Workers { get; set; }
}
