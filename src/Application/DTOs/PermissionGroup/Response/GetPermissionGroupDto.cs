namespace Space.Application.DTOs;

public class GetPermissionGroupDto
{
    public GetPermissionGroupDto()
    {
        Workers = new List<GetWorkerResponseDto>();
    }
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int UserCount { get; set; }
    public ICollection<GetWorkerResponseDto> Workers { get; set; }
}