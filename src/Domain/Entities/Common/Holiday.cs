namespace Space.Domain.Entities;

public class Holiday : BaseEntity
{
    public string Description { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public Guid? ClassId { get; set; }
    public Class? Class { get; set; }
    public int? KometaId { get; set; }
 
}
