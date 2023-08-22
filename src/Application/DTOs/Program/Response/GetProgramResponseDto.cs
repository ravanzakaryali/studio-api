namespace Space.Application.DTOs;

public class GetProgramResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
}
