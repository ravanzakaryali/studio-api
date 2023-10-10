namespace Space.Application.DTOs;

public class GetWorkersByClassResponseDto
{
    public Guid WorkerId { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public int TotalLessonHours { get; set; }
    public string RoleName { get; set; } = null!;
    public Guid? RoleId { get; set; }
    public bool IsAttendance { get; set; }
}
