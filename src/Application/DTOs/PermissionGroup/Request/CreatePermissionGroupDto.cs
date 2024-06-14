namespace Space.Application.DTOs;
public class CreatePermissionGroupDto
{
    public CreatePermissionGroupDto()
    {
        WorkersId = new List<int>();
    }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ICollection<int> WorkersId { get; set; }
}