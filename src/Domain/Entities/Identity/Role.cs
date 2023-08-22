namespace Space.Domain.Entities;

public class Role : IdentityRole<Guid>, IBaseEntity
{
    public Role()
    {
        ClassModulesWorkers = new HashSet<ClassModulesWorker>();
    }
    public bool IsDeleted { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public virtual ICollection<UserRole> UserRoles { get; set; } = null!;
    public ICollection<ClassModulesWorker> ClassModulesWorkers { get; set; } = null!;
}
