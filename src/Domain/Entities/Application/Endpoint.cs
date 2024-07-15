namespace Space.Domain.Entities;

public class Endpoint : BaseEntity
{
    public Endpoint()
    {
        EndpointAccesses = new HashSet<EndpointAccess>();
    }
    public string Path { get; set; } = null!;
    public string HttpMethod { get; set; } = null!;
    public ICollection<EndpointAccess> EndpointAccesses { get; set; }
}