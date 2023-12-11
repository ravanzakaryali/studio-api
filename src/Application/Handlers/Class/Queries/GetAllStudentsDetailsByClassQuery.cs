namespace Space.Application.Handlers;

public record GetAllStudentsDetailsByClassQuery(int Id) : IRequest<IEnumerable<GetStudentsDetailsByClassResponseDto>>;

internal class GetAllStudentsDetailsByClassQueryHandler : IRequestHandler<GetAllStudentsDetailsByClassQuery, IEnumerable<GetStudentsDetailsByClassResponseDto>>
{
    readonly ISpaceDbContext _spaceDbContext;
    public GetAllStudentsDetailsByClassQueryHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetStudentsDetailsByClassResponseDto>> Handle(GetAllStudentsDetailsByClassQuery request, CancellationToken cancellationToken)
    {
        //Todo: Contact null
        Class @class = await _spaceDbContext.Classes
            .Where(c => c.Id == request.Id)
            .Include(c => c.ClassTimeSheets)
            .ThenInclude(c => c.Attendances)
            .Include(c => c.Studies)
            .ThenInclude(c => c.Student)
            .ThenInclude(c => c!.Contact)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                throw new NotFoundException(nameof(Class), request.Id);

        return @class.Studies.Where(c => c.StudyType != StudyType.Completion).Select(study =>
        {
            double? attendancesHour = @class.ClassTimeSheets
            .Where(c => c.Category != ClassSessionCategory.Lab)
            .Select(c => c.Attendances
                .Where(a => a.StudyId == study.Id)
                .Sum(c => c.TotalAttendanceHours))
            .Sum();

            double? totalHour = @class.ClassTimeSheets
            .Where(c => c.Status == ClassSessionStatus.Offline || c.Status == ClassSessionStatus.Online)
            .Sum(s => s.TotalHours);
            GetStudentsDetailsByClassResponseDto studentResponse = new()
            {
                Id = study.Id,
                StudentId = study.StudentId,
                Name = study.Student?.Contact?.Name,
                Surname = study.Student?.Contact?.Surname,
                ClassName = @class.Name,
                Email = study.Student?.Email,
                PhoneNumber = study.Student?.Contact?.Phone,
                FatherName = study.Student?.Contact?.FatherName,
                ArrivalHours = attendancesHour,
                AbsentHours = totalHour - attendancesHour ?? 0,
                Attendance = (totalHour != 0 ? attendancesHour / totalHour * 100 : 0) ?? 0
            };
            return studentResponse;
        });
    }
}