namespace Space.Domain.Entities;

public class Student : BaseAuditableEntity, IKometa
{
    public Student()
    {
        Studies = new HashSet<Study>();
    }
    public int? KometaId { get; set; }
    public string? Email { get; set; }
    public int? ContactId { get; set; }
    public Contact? Contact { get; set; }
    public ICollection<Study> Studies { get; set; }
}
