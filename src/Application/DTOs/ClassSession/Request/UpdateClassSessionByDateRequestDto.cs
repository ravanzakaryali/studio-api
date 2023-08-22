namespace Space.Application.DTOs;

public class UpdateClassSessionByDateRequestDto
{
    public Guid ClassId { get; set; }
    public DateOnly OldDate { get; set; }
    public DateOnly NewDate { get; set; }
    public IEnumerable<UpdateClassSessionDto> Sessions { get; set; } = null!;
}

public class UpdateClassSessionDto
{
    public ClassSessionCategory Category { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}
