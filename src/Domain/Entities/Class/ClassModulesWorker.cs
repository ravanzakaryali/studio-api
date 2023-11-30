namespace Space.Domain.Entities;

public class ClassModulesWorker : BaseAuditableEntity
{
    public Guid ClassId { get; set; }
    public Guid WorkerId { get; set; }
    public Guid ModuleId { get; set; }
    public Guid RoleId { get; set; }
    public Class Class { get; set; } = null!;
    public Worker Worker { get; set; } = null!;
    public Module Module { get; set; } = null!;
    public Role Role { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}
