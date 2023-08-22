namespace Space.Application.DTOs;

public class CreateClassSessionByClassRequestDto
{
    public Guid RoomId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Start { get; set; }
    public TimeOnly End { get; set; }
    public ClassSessionCategory Category { get; set; }
}
