namespace Space.Application.DTOs;

public class GetAllClassDto
{
    public int Id { get; set; }
    public TimeOnly? Start { get; set; }
    public TimeOnly? End { get; set; }
    public string Name { get; set; } = null!;
    public int TotalHours { get; set; }
    public int ProgramId { get; set; }
    public int AttendanceHours { get; set; }
}
