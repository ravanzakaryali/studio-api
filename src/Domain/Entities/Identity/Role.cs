namespace Space.Domain.Entities;

public class Role : IdentityRole<Guid>, IBaseEntity
{
    public Role()
    {
        ClassModulesWorkers = new HashSet<ClassModulesWorker>();
        AttendanceWorkers = new HashSet<AttendanceWorker>();
    }
    public bool IsDeleted { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public ICollection<AttendanceWorker> AttendanceWorkers { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; } = null!;
    public ICollection<ClassModulesWorker> ClassModulesWorkers { get; set; } = null!;
}
