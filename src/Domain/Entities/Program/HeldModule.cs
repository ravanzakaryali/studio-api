namespace Space.Domain.Entities;

public class HeldModule : BaseAuditableEntity
{
    public Guid ModuleId { get; set; }
    public Module Module { get; set; } = null!;
    public int TotalHours { get; set; }
}
