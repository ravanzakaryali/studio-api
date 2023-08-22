namespace Space.Application.DTOs;

public class CreateClassSessionRequestDto
{
    public Guid ClassId { get; set; }
    public IEnumerable<CreateClassSessionDto> Sessions { get; set; } = null!;
}
public class CreateClassSessionDto
{
    public DayOfWeek DayOfWeek { get; set; }
    public ClassSessionCategory Category { get; set; }
    public TimeOnly Start { get; set; }
    public TimeOnly End { get; set; }
}
