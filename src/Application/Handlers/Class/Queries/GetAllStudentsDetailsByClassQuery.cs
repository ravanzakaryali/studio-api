namespace Space.Application.Handlers;

public record GetAllStudentsDetailsByClassQuery(Guid Id) : IRequest<IEnumerable<GetStudentsDetailsByClassResponseDto>>;

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
        Class @class = await _spaceDbContext.Classes
            .Where(c => c.Id == request.Id)
            .Include(c => c.ClassSessions)
            .ThenInclude(c => c.Attendances)
            .Include(c => c.Studies)
            .ThenInclude(c => c.Student)
            .ThenInclude(c => c.Contact)
            .FirstOrDefaultAsync() ??
                throw new NotFoundException(nameof(Class), request.Id);

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
                Email = study.Student?.Contact?.Email,
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