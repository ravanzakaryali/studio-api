namespace Space.Application.DTOs;
public class CreateExtraModuleRequestDto
{
    public string Name { get; set; } = null!;
    public int Hours { get; set; }
    public string Version { get; set; } = null!;
    public int ProgramId { get; set; } 
}