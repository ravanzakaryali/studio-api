namespace Space.Domain.Entities;

public class EndpointDetail : Endpoint
{
    public string? Description { get; set; }
    public int PermissionAccessId { get; set; }
    public PermissionAccess PermissionAccess { get; set; } = null!;
    public int ApplicationModuleId { get; set; }
    public ApplicationModule ApplicationModule { get; set; } = null!;

}