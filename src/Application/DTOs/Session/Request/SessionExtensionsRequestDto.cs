
namespace Space.Application.DTOs;

public class SessionExtensionsRequestDto
{
    public double Hours { get; set; }
    public Guid ClassId { get; set; }
    public Guid RoomId { get; set; }
    public DateOnly? StartDate { get; set; }
    public IEnumerable<CreateClassSessionDto> Sessions { get; set; } = null!;
}
