namespace Space.Application.DTOs;

public class CreateClassRequestDto
{
    public DateOnly StartDate { get; set; }
    public int Week { get; set; }
    public int ProjectId { get; set; }
    public int ProgramId { get; set; }
    public int SessionId { get; set; }
    public int RoomId { get; set; }
}
