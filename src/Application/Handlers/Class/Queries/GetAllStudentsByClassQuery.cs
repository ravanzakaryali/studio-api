using Space.Domain.Entities;

namespace Space.Application.Handlers;

public record GetAllStudentsByClassQuery(Guid Id, DateTime Date) : IRequest<IEnumerable<GetAllStudentByClassResponseDto>>;


internal class GetAllStudentsByClassQueryHandler : IRequestHandler<GetAllStudentsByClassQuery, IEnumerable<GetAllStudentByClassResponseDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly IClassRepository _classRepository;
    readonly IClassSessionRepository _classSessionRepository;

    public GetAllStudentsByClassQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IClassRepository classRepository, IClassSessionRepository classSessionRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _classRepository = classRepository;
        _classSessionRepository = classSessionRepository;
    }

    public async Task<IEnumerable<GetAllStudentByClassResponseDto>> Handle(GetAllStudentsByClassQuery request, CancellationToken cancellationToken)
    {
        Class? @class = await _classRepository.GetAsync(request.Id, tracking: false, "Studies.Student.Contact", "Studies.Attendances", "ClassSessions.Attendances")
         ?? throw new NotFoundException(nameof(Class), request.Id);

        IEnumerable<ClassSession> classSessions = await _classSessionRepository.GetAllAsync(s =>
                                                        s.ClassId == @class.Id && s.Date == request.Date, tracking: false, "Attendances");

        var studentResponses = @class.Studies.Where(c => c.StudyType != StudyType.Completion).Select(study =>
        {
            double? attendancesHour = @class.ClassSessions.Where(c => c.Category != ClassSessionCategory.Lab).Select(c => c.Attendances.Where(a => a.StudyId == study.Id).Sum(c => c.TotalAttendanceHours)).Sum();
            double? totalHour = @class.ClassSessions.Where(c => (c.Status == ClassSessionStatus.Offline || c.Status == ClassSessionStatus.Online) && c.Category != ClassSessionCategory.Lab).Sum(s => s.TotalHour);
            var studentResponse = new GetAllStudentByClassResponseDto
            {
                Name = study.Student?.Contact?.Name,
                Surname = study.Student?.Contact?.Surname,
                EMail = study.Student?.Contact?.Email,
                ClassName = @class.Name,
                Phone = study.Student?.Contact?.Phone,
                Id = study.Student!.Id,
                StudentId = study.Id,
                Attendance = (totalHour != 0 ? attendancesHour / totalHour * 100 : 0) ?? 0,
                Sessions = classSessions.Select(c => new GetAllStudentCategoryDto()
                {
                    ClassSessionCategory = c.Category,
                    Hour = c.Attendances.FirstOrDefault(c => c.StudyId == study.Id)?.TotalAttendanceHours ?? 0,
                    Note = c.Attendances.FirstOrDefault(c => c.StudyId == study.Id)?.Note ?? null
                })
            };
            return studentResponse;
        }).ToList();

        return studentResponses;
    }
}
