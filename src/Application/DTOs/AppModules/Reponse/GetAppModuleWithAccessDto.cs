namespace Space.Application.DTOs;


public class GetAppModuleResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public List<GetAppModuleResponse> AppModules { get; set; } = null!;
}
public class GetAppModuleResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public List<GetAppModuleResponse> SubAppModules { get; set; } = null!;
    public List<GetPermissionAccessDto> PermissionAccesses { get; set; } = null!;
}