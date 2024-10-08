﻿namespace Space.Domain.Entities;

public class Program : BaseAuditableEntity, IKometa
{
    public Program()
    {
        Modules = new HashSet<Module>();
        Classes = new HashSet<Class>();
        ExtraModules = new HashSet<ExtraModule>();
        Projects = new HashSet<Project>();
    }
    public string Color { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int TotalHours { get; set; }
    public ICollection<Module> Modules { get; set; }
    public ICollection<ExtraModule> ExtraModules { get; set; }
    public ICollection<Class> Classes { get; set; }
    public int? KometaId { get; set; }
    public ICollection<Project> Projects { get; set; }
}
