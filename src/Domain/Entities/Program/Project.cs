namespace Space.Domain.Entities;

public class Project : BaseAuditableEntity
{
    public Project()
    {
        Programs = new HashSet<Program>();
        Classes = new HashSet<Class>();
    }
    public string Name { get; set; } = null!;
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public ICollection<Class> Classes { get; set; }
    public ICollection<Program> Programs { get; set; }
}