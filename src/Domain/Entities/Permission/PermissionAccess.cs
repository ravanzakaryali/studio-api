namespace Space.Domain.Entities;
public class PermissionAccess : BaseEntity
{
    public PermissionAccess()
    {
        PermissionLevels = new HashSet<PermissionLevel>();
        EndpointDetails = new HashSet<EndpointAccess>();
    }
    public string Name { get; set; } = null!;
    public ICollection<PermissionLevel> PermissionLevels { get; set; }
    public ICollection<EndpointAccess> EndpointDetails { get; set; }
}
