namespace Space.Application.DTOs;

public class GetClassResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid ProgramId { get; set; }
    public Guid SessionId { get; set; }
    public Guid? RoomId { get; set; }
}
public class GetClassModuleResponseDto
{
    public string ClassName { get; set; } = null!;
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public double Hours { get; set; }
    public string? Version { get; set; }
    public IEnumerable<SubModuleDtoWithWorker>? SubModules { get; set; }
    public ICollection<GetWorkerForClassDto>? Workers { get; set; }
}
