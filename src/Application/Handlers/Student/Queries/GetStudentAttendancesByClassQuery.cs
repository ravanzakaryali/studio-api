using System;

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
        GetStudentAttendancesByClassResponseDto response = new();

        Study study = await _spaceDbContext.Studies
            .Where(q => q.StudentId == request.Id && q.ClassId == request.ClassId)
            .Include(c => c.Class)
            .ThenInclude(c => c.ClassSessions)
            .Include(c => c.Student)
            .ThenInclude(c => c.Contact)
            .Include(c => c.Attendances)
            .ThenInclude(c => c.ClassSession)
            .FirstOrDefaultAsync() ??
                throw new NotFoundException();

        response.EMail = study.Student?.Email;
        response.Phone = study.Student?.Contact?.Phone;
        response.Name = study.Student?.Contact?.Name;
        response.FatherName = study.Student?.Contact?.FatherName;
        response.Surname = study.Student?.Contact?.Surname;
        response.Id = study.StudentId;


        var attendanceList = new List<AttendancesDto>();


        foreach (var item in study.Attendances)
        {
            var attendanceDto = new AttendancesDto()
            {
                AttendanceHours = item.TotalAttendanceHours,
                Date = item.ClassSession.Date,
                LessonHours = item.ClassSession.TotalHour,
                Category = item.ClassSession.Category,
                Note = item.Note

            };

            attendanceList.Add(attendanceDto);
        }
        double? totalHour = (study.Class!.ClassSessions.Where(c => c.Status == ClassSessionStatus.Offline || c.Status == ClassSessionStatus.Online).Sum(c => c.TotalHour));
        double? attendancesHour = study.Attendances.Where(c => c.ClassSession.Category != ClassSessionCategory.Lab).Sum(c => c.TotalAttendanceHours);
        response.AttendancePercent = (totalHour != 0 ? attendancesHour / totalHour * 100 : 0) ?? 0;

        response.Attendances = attendanceList.OrderByDescending(q => q.Date).ToList();


        return response;
    }
}