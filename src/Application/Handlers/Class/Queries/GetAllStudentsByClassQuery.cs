using Space.Domain.Entities;

namespace Space.Application.Handlers;

public record GetAllStudentsByClassQuery(Guid Id, DateTime Date) : IRequest<IEnumerable<GetAllStudentByClassResponseDto>>;


internal class GetAllStudentsByClassQueryHandler : IRequestHandler<GetAllStudentsByClassQuery, IEnumerable<GetAllStudentByClassResponseDto>>
{
    readonly IMapper _mapper;
    readonly ISpaceDbContext _spaceDbContext;

    public GetAllStudentsByClassQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetAllStudentByClassResponseDto>> Handle(GetAllStudentsByClassQuery request, CancellationToken cancellationToken)
    {
        Class? @class = await _spaceDbContext.Classes
            .Where(c => c.Id == request.Id)
            .Include(c => c.Studies)
            .ThenInclude(c => c.Student)
            .ThenInclude(c => c.Contact)
            .Include(c => c.Studies)
            .ThenInclude(c => c.Attendances)
            .Include(c => c.ClassSessions)
            .ThenInclude(c => c.Attendances)
            .FirstOrDefaultAsync() ??
                throw new NotFoundException(nameof(Class), request.Id);


        List<ClassSession> classSessoins = await _spaceDbContext.ClassSessions
            .Where(c => c.ClassId == @class.Id && c.Date == request.Date)
            .AsNoTracking()
            .Include(c => c.Attendances)
            .ToListAsync();

        List<GetAllStudentByClassResponseDto> studentResponses = @class.Studies.Where(c => c.StudyType != StudyType.Completion).Select(study =>
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
                Sessions = classSessoins.Select(c => new GetAllStudentCategoryDto()
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
