namespace Space.Domain.Entities;

public class Support : BaseAuditableEntity
{
    public Support()
    {
        SupportImages = new HashSet<SupportImage>();
    }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
    public ICollection<SupportImage> SupportImages { get; set; }
}
