namespace Space.Domain.Entities;

public class ApplicationModule : BaseEntity
{
    public ApplicationModule()
    {
        PermissionLevels = new HashSet<PermissionLevel>();
        SubModules = new HashSet<ApplicationModule>();
        PermissionGroups = new HashSet<PermissionGroup>();
        Workers = new HashSet<Worker>();
    }
    public string Name { get; set; } = null!;
    public ApplicationModule? ParentModule { get; set; }
    public int? ParentModuleId { get; set; }
    public ICollection<ApplicationModule> SubModules { get; set; }
    public ICollection<PermissionLevel> PermissionLevels { get; set; }
    public ICollection<PermissionGroup> PermissionGroups { get; set; }
    public ICollection<Worker> Workers { get; set; }
}