namespace Space.Application.DTOs;

public class GetAppModuleResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public List<GetAppModuleResponse> SubAppModules { get; set; } = null!;
    public List<GetPermissionAccessDto> PermissionAccesses { get; set; } = null!;
}