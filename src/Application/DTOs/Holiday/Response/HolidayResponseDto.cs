namespace Space.Application.DTOs;

public class HolidayResponseDto
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int? ClassId { get; set; }
}
