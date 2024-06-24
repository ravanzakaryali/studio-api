namespace Space.Domain.Entities;

public class ApplicationModule : BaseEntity
{
    public ApplicationModule()
    {
        SubModules = new HashSet<ApplicationModule>();
        EndpointDetails = new HashSet<EndpointDetail>();
        PermissionGroupPermissionLevelAppModules = new HashSet<PermissionGroupPermissionLevelAppModule>();
        WorkerPermissionLevelAppModules = new HashSet<WorkerPermissionLevelAppModule>();
    }
    public string Name { get; set; } = null!;
    public ApplicationModule? ParentModule { get; set; }
    public string? Description { get; set; }
    public string NormalizedName { get; set; } = null!;
    public int? ParentModuleId { get; set; }
    public ICollection<ApplicationModule> SubModules { get; set; }
    public ICollection<EndpointDetail> EndpointDetails { get; set; }
    public ICollection<PermissionGroupPermissionLevelAppModule> PermissionGroupPermissionLevelAppModules { get; set; }
    public ICollection<WorkerPermissionLevelAppModule> WorkerPermissionLevelAppModules { get; set; }
}