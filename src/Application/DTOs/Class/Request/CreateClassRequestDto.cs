namespace Space.Application.DTOs;

public class CreateClassRequestDto
{
    public string Name { get; set; } = null!;
    public int ProgramId { get; set; }
    public int SessionId { get; set; }
    public int? RoomId { get; set; }
}
