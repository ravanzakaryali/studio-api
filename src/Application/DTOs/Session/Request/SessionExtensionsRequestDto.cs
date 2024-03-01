
namespace Space.Application.DTOs;

public class SessionExtensionsRequestDto
{
    public double Hours { get; set; }
    public int ClassId { get; set; }
    public DateOnly? StartDate { get; set; }
}
