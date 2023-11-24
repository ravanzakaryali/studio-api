namespace Space.Application.DTOs;

public class CreateClassSessionAttendanceRequestDto
{
    public Guid ClassId { get; set; }
    public Guid ModuleId { get; set; }
    public DateOnly Date { get; set; }
    public ICollection<UpdateAttendanceCategorySessionDto> Sessions { get; set; } = null!;
}
public class UpdateAttendanceCategorySessionDto
{
    public ClassSessionStatus Status { get; set; }
    public ClassSessionCategory Category { get; set; }
    public ICollection<CreateAttendanceWorker> AttendancesWorkers { get; set; } = null!;
    public ICollection<UpdateAttendanceDto> Attendances { get; set; } = null!;
}
public class CreateAttendanceWorker
{
    public Guid WorkerId { get; set; }
    public Guid RoleId { get; set; }
    public bool IsAttendance { get; set; }
}
public class UpdateAttendanceDto
{
    public Guid StudentId { get; set; }
    public string? Note { get; set; }
    public int TotalAttendanceHours { get; set; }
}
