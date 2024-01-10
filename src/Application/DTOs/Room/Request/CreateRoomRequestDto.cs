using Space.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Space.Application.DTOs;

public class CreateRoomRequestDto
{
    public string Name { get; set; } = null!;
    public int Capacity { get; set; }
    public RoomType Type { get; set; }
}
