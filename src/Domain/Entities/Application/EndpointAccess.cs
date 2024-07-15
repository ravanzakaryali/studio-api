namespace Space.Domain.Entities;

public class EndpointAccess : BaseEntity
{
    public int EndpointId { get; set; }
    public Endpoint Endpoint { get; set; } = null!;
    public int PermissionAccessId { get; set; }
    public PermissionAccess PermissionAccess { get; set; } = null!;
    public int? ApplicationModuleId { get; set; }
    public ApplicationModule? ApplicationModule { get; set; }

}