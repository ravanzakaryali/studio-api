namespace Space.Domain.Entities;

public class UserRole : IdentityUserRole<int>
{
    public int Id { get; set; }
    public override int UserId { get; set; }
    public virtual User User { get; set; } = null!;
    public override int RoleId { get; set; }
    public virtual Role Role { get; set; } = null!;
}
