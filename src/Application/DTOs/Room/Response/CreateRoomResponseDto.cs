namespace Space.Application.DTOs;

public class CreateRoomResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Capacity { get; set; }
    public RoomType Type { get; set; }

}
