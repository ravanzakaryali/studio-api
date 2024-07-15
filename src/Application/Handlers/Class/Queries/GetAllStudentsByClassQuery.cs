using System.Globalization;
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
            .Include(c => c.Session)
            .ThenInclude(c => c.Details)
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

            double? totalHour = @class.ClassTimeSheets
                .Where(c => c.Status != ClassSessionStatus.Cancelled)
                .Sum(s => s.TotalHours);


            List<GetAllStudentCategoryDto> studentSessions = @class.Session.Details
                    .Where(c => c.DayOfWeek == requestDate.DayOfWeek)
                    .Select(c => new GetAllStudentCategoryDto()
                    {
                        ClassSessionCategory = c.Category,
                        Hour = 0,
                        Note = null
                    }).ToList();

            foreach (GetAllStudentCategoryDto item in studentSessions)
            {
                ClassTimeSheet? findClassTimeSheet = classTimeSheets.FirstOrDefault(c => c.Category == ClassSessionCategory.Theoric) ?? classTimeSheets.FirstOrDefault(c => c.Category == ClassSessionCategory.Lab);
                int hour = findClassTimeSheet?.Attendances.FirstOrDefault(c => c.StudyId == study.Id)?.TotalAttendanceHours ?? 0;
                item.Hour = hour;
                item.ClassSessionCategory = findClassTimeSheet != null ? findClassTimeSheet.Category : ClassSessionCategory.Theoric;
                item.Note = classTimeSheets.FirstOrDefault(c => c.Category == item.ClassSessionCategory)?.Attendances.FirstOrDefault(c => c.StudyId == study.Id)?.Note;
            }


            GetAllStudentByClassResponseDto studentResponse = new()
            {
                Name = study.Student?.Contact?.Name,
                Surname = study.Student?.Contact?.Surname,
                Email = study.Student?.Email,
                ClassName = @class.Name,
                Phone = study.Student?.Contact?.Phone,
                Id = study.Student!.Id,
                StudentId = study.Id,
                Attendance = (totalHour != 0 ? attendancesHour / totalHour * 100 : 0) ?? 0,
                Sessions = studentSessions,
            };
            return studentResponse;
        })

        .ToList();

        return studentResponses
                .OrderBy(c => c.Name, StringComparer.Create(new CultureInfo("az-AZ"), false));
    }
}
