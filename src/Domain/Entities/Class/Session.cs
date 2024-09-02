namespace Space.Domain.Entities;

public class Session : BaseAuditableEntity, IKometa
{
    public Session()
    {
        Details = new HashSet<SessionDetail>();
        Classes = new HashSet<Class>();
    }
    public string Name { get; set; } = null!;
    public int? No { get; set; }
    public ICollection<SessionDetail> Details { get; set; }
    public ICollection<Class> Classes { get; set; }
    public int? KometaId { get; set; }
}
