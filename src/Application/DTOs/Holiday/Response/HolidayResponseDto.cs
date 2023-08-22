namespace Space.Application.DTOs;

public class HolidayResponseDto
{
    public Guid Id { get; set; }
    public string Description { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public Guid? ClassId { get; set; }
}
