namespace Space.Domain.Entities;

public class ClassExtraModulesWorkers : BaseAuditableEntity
{
    public int ClassId { get; set; }
    public int WorkerId { get; set; }
    public int ExtraModuleId { get; set; }
    public int? RoleId { get; set; }
    public Role? Role { get; set; }
    public Class Class { get; set; } = null!;
    public Worker Worker { get; set; } = null!;
    public ExtraModule ExtraModule { get; set; } = null!;
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}