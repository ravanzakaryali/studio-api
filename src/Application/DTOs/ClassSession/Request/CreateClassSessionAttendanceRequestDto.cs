namespace Space.Application.DTOs;

public class CreateClassSessionAttendanceRequestDto
{
    public Guid ClassId { get; set; }
    public DateOnly Date { get; set; }
    public ICollection<CreateAttendanceModuleRequestDto> HeldModules { get; set; } = null!;
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
    public int TotalHours { get; set; }
    public int TotalMinutes { get; set; }
    public AttendanceStatus AttendanceStatus { get; set; }
}
public class UpdateAttendanceDto
{
    public Guid StudentId { get; set; }
    public string? Note { get; set; }
    public int TotalAttendanceHours { get; set; }
}
