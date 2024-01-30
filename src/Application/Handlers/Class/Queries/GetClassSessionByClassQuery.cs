namespace Space.Application.Handlers;

public record GetClassSessionByClassQuery(int Id, DateTime Date) : IRequest<IEnumerable<GetClassSessionClassByDayResponseDto>>;
internal class GetClassSessionByClassQueryHandler : IRequestHandler<GetClassSessionByClassQuery, IEnumerable<GetClassSessionClassByDayResponseDto>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetClassSessionByClassQueryHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetClassSessionClassByDayResponseDto>> Handle(GetClassSessionByClassQuery request, CancellationToken cancellationToken)
    {
        Class? @class = await _spaceDbContext.Classes
            .Where(c => c.Id == request.Id)
            .Include(c => c.ClassSessions)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                throw new NotFoundException();

        DateOnly requestDate = DateOnly.FromDateTime(request.Date);

        List<ClassSession> classSessions = @class.ClassSessions.Where(c => c.Date == requestDate).ToList();
        int totalHour = classSessions.Sum(c => c.TotalHours);
        return classSessions.Select(session => new GetClassSessionClassByDayResponseDto()
        {
            ClassName = @class.Name,
            Category = session.Category,
            EndTime = session.EndTime,
            StartTime = session.StartTime,
            Hours = totalHour,
        });
    }
}
