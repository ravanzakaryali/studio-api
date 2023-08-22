namespace Space.Domain.Entities;

public class Module : BaseAuditableEntity
{
    public Module()
    {
        ClassModulesWorkers = new HashSet<ClassModulesWorker>();
        ClassSessions = new HashSet<ClassSession>();
        SubModules = new List<Module>();
    }
    public double Hours { get; set; }
    public Guid? ProgramId { get; set; }
    public Program? Program { get; set; }
    public string Name { get; set; } = null!;
    public string? Version { get; set; }
    public Guid? TopModuleId { get; set; }
    public Module? TopModule { get; set; }
    public List<Module>? SubModules { get; set; }
    public ICollection<ClassModulesWorker> ClassModulesWorkers { get; set; }
    public ICollection<ClassSession> ClassSessions { get; set; }
}
