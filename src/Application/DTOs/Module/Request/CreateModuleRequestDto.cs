namespace Space.Application.DTOs;

public class CreateModuleRequestDto
{
    public string Name { get; set; } = null!;
    public int Hours { get; set; }
    public string Version { get; set; } = null!;
    public List<CreateSubModuleRequestDto>? SubModules { get; set; }
}
public class CreateSubModuleRequestDto
{
    public string Name { get; set; } = null!;
    public int Hours { get; set; }
    public string Version { get; set; } = null!;
}
