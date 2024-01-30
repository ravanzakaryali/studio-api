namespace Space.Domain.Entities;

public class UserRole : IdentityUserRole<Guid>
{
    public Guid Id { get; set; }
    public override Guid UserId { get; set; }
    public virtual User User { get; set; }
    public override Guid RoleId { get; set; }
    public virtual Role Role { get; set; }
}
