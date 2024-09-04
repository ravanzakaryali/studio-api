namespace Space.Application.DTOs;

public class GetClassDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public class GetClassDetailDto
{
    public int Id { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int StudentCount { get; set; }
}