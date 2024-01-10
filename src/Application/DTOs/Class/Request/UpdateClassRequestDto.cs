namespace Space.Application.DTOs;

public class UpdateClassRequestDto
{
    public string Name { get; set; } = null!;
    public int SessionId { get; set; }
    public int ProgramId { get; set; }
    public int? RoomId { get; set; }
}
