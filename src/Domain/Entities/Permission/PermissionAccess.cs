namespace Space.Domain.Entities;
public class PermissionAccess : BaseEntity
{
    public PermissionAccess()
    {
        PermissionLevels = new HashSet<PermissionLevel>();
        EndpointDetails = new HashSet<EndpointDetail>();
    }
    public string Name { get; set; } = null!;
    public ICollection<PermissionLevel> PermissionLevels { get; set; }
    public ICollection<EndpointDetail> EndpointDetails { get; set; }
}
