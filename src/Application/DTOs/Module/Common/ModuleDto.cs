namespace Space.Application.DTOs;

public class ModuleDto
{
    public string Id {get;set;} = null!;
    public string Name { get; set; } = null!;
    public string Version { get; set; } = null!;
    public double Hours { get; set; }
    public IEnumerable<CreateSubModuleDto>? SubModules { get; set; }
}
