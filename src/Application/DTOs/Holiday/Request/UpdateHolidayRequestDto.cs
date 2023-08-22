namespace Space.Application.DTOs;

public class UpdateHolidayRequestDto
{
    public string Description { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public Guid? ClassId { get; set; }
}
