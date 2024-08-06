namespace Space.Application.DTOs;

public class StartAttendanceRequestDto
{
    public StartAttendanceRequestDto()
    {
        ModulesId = new HashSet<int>();
    }
    public int ClassId { get; set; }
    public DateOnly Date { get; set; }
    public ClassSessionCategory SessionCategory { get; set; }
    public ICollection<int> ModulesId { get; set; }
    public int WorkerId { get; set; }
}