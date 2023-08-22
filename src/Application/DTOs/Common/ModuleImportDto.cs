namespace Space.Application.DTOs;

public class ModuleImportDto
{
    public string Program { get; set; } = null!;
    public double Version { get; set; }
    public string Name { get; set; } = null!;
    public double Hour { get; set; }
    public List<ModuleImportSubDto> Programs { get; set; }
}

public class ModuleImportSubDto
{
    public string Program { get; set; } = null!;
    public double Version { get; set; }
    public string Name { get; set; } = null!;
    public double Hour { get; set; }
}