namespace Space.Domain.Entities;

public class University : BaseEntity, IKometa
{
    public string Name { get; set; } = null!;
    public int? KometaId { get; set; }
}
