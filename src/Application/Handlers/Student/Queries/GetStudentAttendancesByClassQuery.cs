using System;

namespace Space.Application.Handlers;


public record GetStudentAttendancesByClassQuery(Guid Id, Guid classId) : IRequest<GetStudentAttendancesByClassResponseDto>;


internal class GetStudentAttendancesByClassQueryHandler : IRequestHandler<GetStudentAttendancesByClassQuery, GetStudentAttendancesByClassResponseDto>
{

    readonly IStudyRepository _studyRepository;
    readonly IUnitOfWork _unitOfWork;

    public GetStudentAttendancesByClassQueryHandler(
        IUnitOfWork unitOfWork,
        IStudyRepository studyRepository)
    {
        _unitOfWork = unitOfWork;
        _studyRepository = studyRepository;
    }

    public async Task<GetStudentAttendancesByClassResponseDto> Handle(GetStudentAttendancesByClassQuery request, CancellationToken cancellationToken)
    {
        GetStudentAttendancesByClassResponseDto response = new();

        Study study = await _studyRepository.GetAsync(q => q.StudentId == request.Id && q.ClassId == request.classId, false, "Class.ClassSessions", "Student.Contact", "Attendances.ClassSession") ?? throw new NotFoundException();

        response.EMail = study.Student?.Email;
        response.Phone = study.Student.Contact?.Phone;
        response.Name = study.Student.Contact?.Name;
        response.FatherName = study.Student.Contact?.FatherName;
        response.Surname = study.Student.Contact?.Surname;
        response.Id = study.Student.Id;


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
        double attendancesHour = study.Attendances.Where(c => c.ClassSession.Category != ClassSessionCategory.Lab).Sum(c => c.TotalAttendanceHours);
        response.AttendancePercent = (totalHour != 0 ? attendancesHour / totalHour * 100 : 0) ?? 0;

        response.Attendances = attendanceList.OrderByDescending(q => q.Date).ToList();


        return response;
    }
}