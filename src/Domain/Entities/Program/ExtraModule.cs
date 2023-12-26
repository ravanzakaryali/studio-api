namespace Space.Domain.Entities;

public class ExtraModule : BaseAuditableEntity
{
    public double Hours { get; set; }
    public int? ProgramId { get; set; }
    public Program? Program { get; set; }
    public string Name { get; set; } = null!;
    public string? Version { get; set; }
}