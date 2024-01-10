namespace Space.Application.DTOs;

public class CreateHolidayRequestDto
{
    public string Description { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}
