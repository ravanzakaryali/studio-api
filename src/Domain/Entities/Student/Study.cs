namespace Space.Domain.Entities;

public class Study : BaseAuditableEntity, IKometa
{
    public int? KometaId { get; set; }
    public string Status { get; set; } = null!;

    public StudyType? StudyType { get; set; }
    public Guid ClassId { get; set; }
    public Class Class { get; set; } = null!;
    //Hack: Student id nullable
    public Guid? StudentId { get; set; }
    public Student? Student { get; set; }
    public ICollection<Attendance> Attendances { get; set; } = null!;
}
