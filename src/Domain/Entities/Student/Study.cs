namespace Space.Domain.Entities;

public class Study : BaseAuditableEntity, IKometa
{
    public int? KometaId { get; set; }
    public string Status { get; set; } = null!;

    public StudyType? StudyType { get; set; }
    public int? ClassId { get; set; }
    public Class? Class { get; set; }
    public int? StudentId { get; set; }
    public Student? Student { get; set; }
    public ICollection<Attendance> Attendances { get; set; } = null!;
}
