
namespace Space.Application.DTOs;

public class GetRoomResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Capacity { get; set; }
    public RoomType Type { get; set; }
}
