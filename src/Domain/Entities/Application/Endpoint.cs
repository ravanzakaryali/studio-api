namespace Space.Domain.Entities;

public class Endpoint : BaseEntity
{
    public string Path { get; set; } = null!;
    public string HttpMethod { get; set; } = null!;
}