namespace Space.Domain.Entities;

public class SupportCategory : BaseEntity
{
    public SupportCategory()
    {
        Supports = new HashSet<Support>();
    }
    public string Name { get; set; } = null!;
    public ICollection<Support> Supports { get; set; }
}
