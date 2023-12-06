namespace Space.Application.DTOs;

public class CreateModuleRequestDto
{
    public string Name { get; set; } = null!;
    public List<string>? SubModules { get; set; }
}
