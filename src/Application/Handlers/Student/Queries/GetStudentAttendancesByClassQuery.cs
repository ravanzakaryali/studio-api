namespace Space.Application.Handlers;


public record GetStudentAttendancesByClassQuery(Guid Id, Guid ClassId) : IRequest<GetStudentAttendancesByClassResponseDto>;


internal class GetStudentAttendancesByClassQueryHandler : IRequestHandler<GetStudentAttendancesByClassQuery, GetStudentAttendancesByClassResponseDto>
{

    readonly ISpaceDbContext _spaceDbContext;

    public GetStudentAttendancesByClassQueryHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<GetStudentAttendancesByClassResponseDto> Handle(GetStudentAttendancesByClassQuery request, CancellationToken cancellationToken)
    {
        //Todo: Class session
        Study study = await _spaceDbContext.Studies
            .Where(q => q.StudentId == request.Id && q.ClassId == request.ClassId)
            .Include(c => c.Class)
            .ThenInclude(c => c!.ClassSessions)
            .Include(c => c.Student)
            .ThenInclude(c => c!.Contact)
            .Include(c => c.Attendances)
            .ThenInclude(c => c.ClassTimeSheets)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                throw new NotFoundException();

        GetStudentAttendancesByClassResponseDto response = new()
        {
            Email = study.Student?.Email,
            Phone = study.Student?.Contact?.Phone,
            Name = study.Student?.Contact?.Name,
            FatherName = study.Student?.Contact?.FatherName,
            Surname = study.Student?.Contact?.Surname,
            Id = study.StudentId
        };

        List<AttendancesDto> attendanceList = new();
        foreach (Attendance item in study.Attendances)
        {
            AttendancesDto attendanceDto = new()
            {
                AttendanceHours = item.TotalAttendanceHours,
                Date = item.ClassTimeSheets.Date,
                LessonHours = item.ClassTimeSheets.TotalHours,
                Category = item.ClassTimeSheets.Category,
                Note = item.Note

            };
            attendanceList.Add(attendanceDto);
        }
        double? totalHour = (study.Class.ClassTimeSheets.Where(c => c.Status == ClassSessionStatus.Offline || c.Status == ClassSessionStatus.Online).Sum(c => c.TotalHours));
        double? attendancesHour = study.Attendances.Where(c => c.ClassTimeSheets.Category != ClassSessionCategory.Lab).Sum(c => c.TotalAttendanceHours);
        response.AttendancePercent = (totalHour != 0 ? attendancesHour / totalHour * 100 : 0) ?? 0;
        response.Attendances = attendanceList.OrderByDescending(q => q.Date).ToList();
        return response;
    }
}