public class GetAttendanceSessionDto
{
    public GetClassAttendanceDto Class { get; set; } = null!;
    public TimeOnly StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public int ClassTimeSheetId { get; set; }
    public ClassSessionCategory Category { get; set; }
    public IEnumerable<GetHeldModulesDto> HeldModules { get; set; } = null!;
    public IEnumerable<GetAllStudentByClassResponseDto> Students { get; set; } = null!;

}