
namespace Space.Application.DTOs;

public class SessionExtensionsRequestDto
{
    public double Hours { get; set; }
    public int ClassId { get; set; }
    public int RoomId { get; set; }
    public DateOnly? StartDate { get; set; }
    public IEnumerable<CreateClassSessionDto> Sessions { get; set; } = null!;
}
