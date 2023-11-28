namespace Space.Application.DTOs;

public class GetClassResponseDto
{
    public Guid Id { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public Guid ProgramId { get; set; }
    public Guid SessionId { get; set; }
    public Guid? RoomId { get; set; }
}
public class GetClassModuleResponseDto
{
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public double Hours { get; set; }
    public string? Version { get; set; }
    public List<SubModuleDtoWithWorker>? SubModules { get; set; }
    public ICollection<GetWorkerForClassDto>? Workers { get; set; }
}
