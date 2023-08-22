namespace Space.Application.DTOs;

public class CreateClassRequestDto
{
    public string Name { get; set; } = null!;
    public Guid ProgramId { get; set; }
    public Guid SessionId { get; set; }
    public Guid? RoomId { get; set; }
}
