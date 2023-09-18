namespace Space.Application.DTOs;

public class CreateClassSessionAttendanceRequestDto
{
    public Guid ClassId { get; set; }
    public Guid ModuleId { get; set; }
    public DateTime Date { get; set; }
    public ICollection<UpdateAttendanceCategorySessionDto> Sessions { get; set; }
}
public class UpdateAttendanceCategorySessionDto
{
    public ClassSessionStatus Status { get; set; }
    public ClassSessionCategory Category { get; set; }
    public ICollection<CreateAttendanceWorker> AttendancesWorkers { get; set; }
    public ICollection<UpdateAttendanceDto> Attendances { get; set; }
}
public class CreateAttendanceWorker
{
    public Guid WorkerId { get; set; }
    public RoleEnum Role { get; set; }
    public bool IsAttendance { get; set; }
}
public enum RoleEnum
{
    Mentor = 0,
    Muellim = 1,
    Admin = 2,
}
public class UpdateAttendanceDto
{
    public Guid StudentId { get; set; }
    public string? Note { get; set; }
    public int TotalAttendanceHours { get; set; }
}
