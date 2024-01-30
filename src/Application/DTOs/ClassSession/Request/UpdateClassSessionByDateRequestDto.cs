namespace Space.Application.DTOs;

public class UpdateClassSessionByDateRequestDto
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public IEnumerable<CreateClassSessionDto> Sessions { get; set; } = null!;
}


public class UpdateClassSessionDto
{
    public ClassSessionCategory Category { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}
