namespace Space.Application.DTOs;

public class UpdateClassRequestDto
{
    public string Name { get; set; } = null!;
    public Guid SessionId { get; set; }
    public Guid ProgramId { get; set; }
    public Guid? RoomId { get; set; }
}
