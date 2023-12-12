namespace Space.Application.DTOs;

public class GetAllClassDto
{
    public int Id { get; set; }
    public TimeOnly? Start { get; set; } 
    public TimeOnly? End { get; set; }
    public string Name { get; set; } = null!;
}
