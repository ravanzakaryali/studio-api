using Space.Domain.Entities;

namespace Space.Application.Handlers;

public record GetAllStudentsByClassQuery(int Id, DateTime Date) : IRequest<IEnumerable<GetAllStudentByClassResponseDto>>;


internal class GetAllStudentsByClassQueryHandler : IRequestHandler<GetAllStudentsByClassQuery, IEnumerable<GetAllStudentByClassResponseDto>>
{
    readonly IMapper _mapper;
    readonly ISpaceDbContext _spaceDbContext;

    public GetAllStudentsByClassQueryHandler(
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetAllStudentByClassResponseDto>> Handle(GetAllStudentsByClassQuery request, CancellationToken cancellationToken)
    {
        //Todo: Contact null
        Class? @class = await _spaceDbContext.Classes
            .Where(c => c.Id == request.Id)
            .Include(c => c.Studies)
            .ThenInclude(c => c.Student)
            .ThenInclude(c => c!.Contact)
            .Include(c => c.Studies)
            .ThenInclude(c => c.Attendances)
            .Include(c => c.ClassSessions)
            .Include(c => c.ClassTimeSheets)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                throw new NotFoundException(nameof(Class), request.Id);


        DateOnly requestDate = DateOnly.FromDateTime(request.Date);

        List<ClassTimeSheet> classTimeSheets = await _spaceDbContext.ClassTimeSheets
            .Where(c => c.ClassId == @class.Id && c.Date == requestDate)
            .AsNoTracking()
            .Include(c => c.Attendances)
            .ToListAsync(cancellationToken: cancellationToken);

        //Todo: Error
        List<GetAllStudentByClassResponseDto> studentResponses = @class.Studies
            .Where(c => c.StudyType != StudyType.Completion)
            .Select(study =>
        {
            double? attendancesHour = @class.ClassTimeSheets
                .Where(c => c.Category != ClassSessionCategory.Lab)
                .Select(c => c.Attendances
                                    .Where(a => a.StudyId == study.Id)
                                    .Sum(c => c.TotalAttendanceHours))
                .Sum();

            double? totalHour = @class.ClassSessions
                .Where(c => (c.Status == ClassSessionStatus.Offline || c.Status == ClassSessionStatus.Online) && c.Category != ClassSessionCategory.Lab)
                .Sum(s => s.TotalHours);


            IEnumerable<GetAllStudentCategoryDto> studentSessions = @class.ClassSessions
            .Where(c => c.Date == requestDate)
            .Select(c => new GetAllStudentCategoryDto()
            {
                ClassSessionCategory = c.Category,
                Hour = 0,
                Note = null
            });
            if (classTimeSheets.Any())
            {
                studentSessions = classTimeSheets.Select(c => new GetAllStudentCategoryDto()
                {
                    ClassSessionCategory = c.Category,
                    Hour = c.Attendances.FirstOrDefault(c => c.StudyId == study.Id)?.TotalAttendanceHours,
                    Note = c.Attendances.FirstOrDefault(c => c.StudyId == study.Id)?.Note
                });
            }

            GetAllStudentByClassResponseDto studentResponse = new()
            {
                Name = study.Student?.Contact?.Name,
                Surname = study.Student?.Contact?.Surname,
                EMail = study.Student?.Contact?.Email,
                ClassName = @class.Name,
                Phone = study.Student?.Contact?.Phone,
                Id = study.Student!.Id,
                StudentId = study.Id,
                Attendance = (totalHour != 0 ? attendancesHour / totalHour * 100 : 0) ?? 0,
                Sessions = studentSessions,
            };
            return studentResponse;
        }).ToList();

        return studentResponses;
    }
}
