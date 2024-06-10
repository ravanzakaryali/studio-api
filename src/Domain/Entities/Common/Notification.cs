namespace Space.Domain.Entities;

public class Notification : BaseAuditableEntity
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public int? FromUserId { get; set; }
    public User? FromUser { get; set; }
    public bool AllUsers { get; set; }
    public bool IsRead { get; set; }
    
}