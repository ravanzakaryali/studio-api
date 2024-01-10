
namespace Space.Domain.Entities;

public class Contact : BaseAuditableEntity, IKometa
{
    public int? KometaId { get; set; }
    public Student? Student { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? FatherName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
}
