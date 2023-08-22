namespace Space.Application.DTOs;

public class UpdateRoomRequestDto 
{
    public string Name { get; set; } = null!;
    public int Capacity { get; set; }
    public RoomType Type { get; set; }
}
