namespace Space.Domain.Entities;

public class ApplicationModule : BaseEntity
{
    public ApplicationModule()
    {
        SubModules = new HashSet<ApplicationModule>();
        EndpointDetails = new HashSet<EndpointDetail>();
        PermissionGroupAppModulePermissionLevels = new HashSet<PermissionGroupAppModulePermissionLevel>();
    }
    public string Name { get; set; } = null!;
    public ApplicationModule? ParentModule { get; set; }
    public int? ParentModuleId { get; set; }
    public ICollection<ApplicationModule> SubModules { get; set; }
    public ICollection<EndpointDetail> EndpointDetails { get; set; }
    public ICollection<PermissionGroupAppModulePermissionLevel> PermissionGroupAppModulePermissionLevels { get; set; }
}