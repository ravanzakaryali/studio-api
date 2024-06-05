namespace Space.Application.DTOs;



public class PermissionGroupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}
public class GetPermissionGroupDto : PermissionGroupDto
{
    public GetPermissionGroupDto()
    {
        Workers = new List<GetWorkerResponseDto>();
    }
    public int UserCount { get; set; }
    public ICollection<GetWorkerResponseDto> Workers { get; set; }
}