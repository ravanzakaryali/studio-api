namespace Space.Application.DTOs;

public class GetClassModuleWorkersResponse
{
    public Guid Id { get; set; }
    public string ClassName { get; set; } = null!;
    public string ProgramName { get; set; } = null!;
    public Guid ProgramId { get; set; }
    public string SessionName { get; set; } = null!;
    public int TotalModules { get; set; }
    public int TotalHour { get; set; }
    public int CurrentHour { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public IEnumerable<GetWorkerForClassDto> Workers { get; set; } = null!;
}

public class GetClassModuleWorkers
{
    public Guid Id { get; set; }
    public string ClassName { get; set; } = null!;
    public string ProgramName { get; set; } = null!;
    public Guid ProgramId { get; set; }
    public string SessionName { get; set; } = null!;
    public int TotalModules { get; set; }
    public int TotalHour { get; set; }
    public int CurrentHour { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public IEnumerable<ClassModulesWorker> ClassModulesWorkers { get; set; } = null!;
}