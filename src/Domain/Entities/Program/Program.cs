namespace Space.Domain.Entities;

public class Program : BaseAuditableEntity, IKometa
{
    public Program()
    {
        Modules = new HashSet<Module>();
        Classes = new HashSet<Class>();
    }
    public string Color { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int TotalHours { get; set; }
    public ICollection<Module> Modules { get; set; }
    public ICollection<Class> Classes { get; set; }
    public int? KometaId { get; set; }
}
