namespace Space.Application.DTOs.Worker;

public class CreateRequestWorkerDto
{
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Fincode { get; set; } = null!;
    public IEnumerable<int>? GroupsId { get; set; }
}
