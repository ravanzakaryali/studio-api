namespace Space.Domain.Entities;

public class HeldModule : BaseAuditableEntity
{
    public int? ModuleId { get; set; }
    public Module? Module { get; set; }
    public int? ExtraModuleId { get; set; }
    public ExtraModule? ExtraModule { get; set; }
    public int ClassTimeSheetId { get; set; }
    public ClassTimeSheet ClassTimeSheet { get; set; } = null!;
    public int TotalHours { get; set; }
}
