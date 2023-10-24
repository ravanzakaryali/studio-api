namespace Space.Application.Handlers;

public record GetAllStudentsDetailsByClassQuery(Guid Id) : IRequest<IEnumerable<GetStudentsDetailsByClassResponseDto>>;

internal class GetAllStudentsDetailsByClassQueryHandler : IRequestHandler<GetAllStudentsDetailsByClassQuery, IEnumerable<GetStudentsDetailsByClassResponseDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IClassRepository _classRepository;
    public GetAllStudentsDetailsByClassQueryHandler(IUnitOfWork unitOfWork, IClassRepository classRepository)
    {
        _unitOfWork = unitOfWork;
        _classRepository = classRepository;
    }

    public async Task<IEnumerable<GetStudentsDetailsByClassResponseDto>> Handle(GetAllStudentsDetailsByClassQuery request, CancellationToken cancellationToken)
    {
        Class @class = await _classRepository.GetAsync(request.Id, tracking: false, "ClassSessions.Attendances", "Studies.Student.Contact")
            ?? throw new NotFoundException(nameof(Class), request.Id);
        return @class.Studies.Where(c => c.StudyType != StudyType.Completion).Select(study =>
        {
            double? attendancesHour = @class.ClassSessions.Where(c => c.Category != ClassSessionCategory.Lab).Select(c => c.Attendances.Where(a => a.StudyId == study.Id).Sum(c => c.TotalAttendanceHours)).Sum();
            double? totalHour = @class.ClassSessions.Where(c => c.Status == ClassSessionStatus.Offline || c.Status == ClassSessionStatus.Online).Sum(s => s.TotalHour);
            var studentResponse = new GetStudentsDetailsByClassResponseDto
            {
                Id = study.Id,
                StudentId = study.StudentId,
                Name = study.Student?.Contact?.Name,
                Surname = study.Student?.Contact?.Surname,
                ClassName = @class.Name,
                ArrivalHours = attendancesHour,
                AbsentHours = totalHour - attendancesHour ?? 0,
                Attendance = (totalHour != 0 ? attendancesHour / totalHour * 100 : 0) ?? 0
            };
            return studentResponse;
        });
    }
}