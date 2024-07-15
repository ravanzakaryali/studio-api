namespace Space.Application.DTOs;
public class LoginUserPermissionDto
{
    public int AppModuleId { get; set; }
    public string Name { get; set; } = null!;
    public string NormalizedName { get; set; } = null!;
    public List<string> Permissions { get; set; } = new List<string>();
}