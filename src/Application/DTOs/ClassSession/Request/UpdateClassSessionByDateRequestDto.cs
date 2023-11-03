namespace Space.Application.DTOs;

public class UpdateClassSessionByDateRequestDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public IEnumerable<CreateClassSessionDto> Sessions { get; set; } = null!;
}


public class UpdateClassSessionDto
{
    public ClassSessionCategory Category { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}
