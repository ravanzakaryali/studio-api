namespace Space.Domain.Entities;

public class ApplicationModule : BaseEntity
{
    public ApplicationModule()
    {
        SubModules = new List<ApplicationModule>();
    }
    public string Name { get; set; } = null!;
    public ApplicationModule? ParentModule { get; set; }
    public int? ParentModuleId { get; set; }
    public ICollection<ApplicationModule> SubModules { get; set; }
}