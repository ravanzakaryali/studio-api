namespace Space.Domain.Entities;

public class Module : BaseAuditableEntity
{
    public Module()
    {
        ClassModulesWorkers = new HashSet<ClassModulesWorker>();
        SubModules = new List<Module>();
    }
    public double Hours { get; set; }
    public int? ProgramId { get; set; }
    public Program? Program { get; set; }
    public bool IsQuestionnaire { get; set; }
    public string Name { get; set; } = null!;
    public string? Version { get; set; }
    public int? TopModuleId { get; set; }
    public Module? TopModule { get; set; }
    public List<Module>? SubModules { get; set; }
    public ICollection<ClassModulesWorker> ClassModulesWorkers { get; set; }
}
