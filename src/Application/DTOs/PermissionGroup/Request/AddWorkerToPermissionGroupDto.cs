namespace Space.Application.DTOs;
public class AddWorkerToPermissionGroupDto
{
    public AddWorkerToPermissionGroupDto()
    {
        WorkerIds = new List<int>();
    }
    public IEnumerable<int> WorkerIds { get; set; }
}