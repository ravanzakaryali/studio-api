namespace Space.Application.DTOs;

public class CreateClassModuleRequestDto
{
    public int ModuleId { get; set; }
    public int? WorkerId { get; set; }
    public int? RoleId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}


public class UpdateClassModuleDto
{
    public int ModuleId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public IEnumerable<ModulesWorkersDto> Workers { get; set; } = null!;
}

public class CreateClassExtraModuleRequestDto
{
    public int ExtraModuleId { get; set; }
    public IEnumerable<ModulesWorkersDto> Workers { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}

public class CreateClassNewExtraModuleRequestDto
{
    public string ExtraModuleName { get; set; } = null!;
    public int Hours { get; set; }
    public IEnumerable<ModulesWorkersDto> Workers { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}
public class ModulesWorkersDto
{
    public int WorkerId { get; set; }
    public int RoleId { get; set; }
}