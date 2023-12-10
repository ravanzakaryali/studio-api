namespace Space.Application.Handlers;

public record GetClassSessionByClassQuery(Guid Id, DateTime Date) : IRequest<IEnumerable<GetClassSessionByClassResponseDto>>;
internal class GetClassSessionByClassQueryHandler : IRequestHandler<GetClassSessionByClassQuery, IEnumerable<GetClassSessionByClassResponseDto>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetClassSessionByClassQueryHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetClassSessionByClassResponseDto>> Handle(GetClassSessionByClassQuery request, CancellationToken cancellationToken)
    {
        Class? @class = await _spaceDbContext.Classes
            .Where(c => c.Id == request.Id)
            .Include(c => c.ClassGenerateSessions)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                throw new NotFoundException();

        DateOnly requestDate = DateOnly.FromDateTime(request.Date);

        List<ClassGenerateSession> classSessions = @class.ClassGenerateSessions.Where(c => c.Date == requestDate).ToList();
        int totalHour = classSessions.Sum(c => c.TotalHours);
        return classSessions.Select(session => new GetClassSessionByClassResponseDto()
        {
            ClassName = @class.Name,
            Category = session.Category,
            EndTime = session.EndTime,
            StartTime = session.StartTime,
            TotalHours = totalHour,
        });
    }
}
