namespace Space.Domain.Entities;

public class Support : BaseAuditableEntity
{
    public Support()
    {
        SupportImages = new HashSet<SupportImage>();
    }
    public string Title { get; set; } = null!;
    public int? ClassId { get; set; }
    public Class? Class { get; set; }
    public string? Description { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
    public SupportStatus Status { get; set; }
    public int? SupportCategoryId { get; set; }
    public SupportCategory? SupportCategory { get; set; }
    public string? Note { get; set; }
    public ICollection<SupportImage> SupportImages { get; set; }
}
