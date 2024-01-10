namespace Space.Domain.Entities;

public class ExtraModule : BaseAuditableEntity
{
    public ExtraModule()
    {
        HeldModules = new HashSet<HeldModule>();
        ClassExtraModulesWorkers = new HashSet<ClassExtraModulesWorkers>();
    }
    public double Hours { get; set; }
    public int? ProgramId { get; set; }
    public Program? Program { get; set; }
    public string Name { get; set; } = null!;
    public string? Version { get; set; }
    public ICollection<HeldModule> HeldModules { get; set; }
    public ICollection<ClassExtraModulesWorkers> ClassExtraModulesWorkers { get; set; }
}