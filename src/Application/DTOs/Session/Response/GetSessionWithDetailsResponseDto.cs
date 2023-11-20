namespace Space.Application.DTOs;

public class GetSessionWithDetailsResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<GetDetailsResponseDto>? Details { get; set; }
}

public class GetDetailsResponseDto
{
    public Guid Id { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public ClassSessionCategory Category { get; set; }
    public int TotalHours { get; set; }
}