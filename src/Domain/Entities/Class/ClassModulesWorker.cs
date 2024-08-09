namespace Space.Domain.Entities;

public class ClassModulesWorker : BaseAuditableEntity
{
    public int ClassId { get; set; }
    public int WorkerId { get; set; }
    public int ModuleId { get; set; }
    public int? RoleId { get; set; }
    public Role? Role { get; set; }
    public WorkerType WorkerType { get; set; }
    public Class Class { get; set; } = null!;
    public Worker Worker { get; set; } = null!;
    public Module Module { get; set; } = null!;
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}
