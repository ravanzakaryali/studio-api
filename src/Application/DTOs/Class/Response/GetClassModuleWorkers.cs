namespace Space.Application.DTOs;

public class GetClassModuleWorkersResponse
{
    public int Id { get; set; }
    public string ClassName { get; set; } = null!;
    public string ProgramName { get; set; } = null!;
    public int ProgramId { get; set; }
    public string SessionName { get; set; } = null!;
    public int TotalModules { get; set; }
    public int StudyCount { get; set; }
    public int TotalHour { get; set; }
    public int CurrentHour { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public IEnumerable<GetWorkerForClassDto> Workers { get; set; } = null!;
}

public class GetClassModuleWorkers
{
    public int Id { get; set; }
    public string ClassName { get; set; } = null!;
    public string ProgramName { get; set; } = null!;
    public int ProgramId { get; set; }
    public string SessionName { get; set; } = null!;
    public int TotalModules { get; set; }
    public int StudyCount { get; set; }
    public int TotalHour { get; set; }
    public int CurrentHour { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public IEnumerable<ClassModulesWorker> ClassModulesWorkers { get; set; } = null!;
}