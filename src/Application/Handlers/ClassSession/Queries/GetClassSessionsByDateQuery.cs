namespace Space.Application.Handlers;

public record GetClassSessionsByDateQuery(Guid Id, DateTime Date) : IRequest<IEnumerable<GetClassSessionsByDateResponseDto>>;


internal class GetClassSessionsByDateQueryHandler : IRequestHandler<GetClassSessionsByDateQuery, IEnumerable<GetClassSessionsByDateResponseDto>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetClassSessionsByDateQueryHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetClassSessionsByDateResponseDto>> Handle(GetClassSessionsByDateQuery request, CancellationToken cancellationToken)
    {
        List<ClassSession> classSessions = await _spaceDbContext.ClassSessions
            .Where(c => c.Date.Year == request.Date.Year &&
                      c.Date.Day == request.Date.Day &&
                      c.Date.Month == request.Date.Month &&
                      c.ClassId == request.Id)
            .ToListAsync();


        IEnumerable<GetClassSessionsByDateResponseDto> response = classSessions.Select(q => new GetClassSessionsByDateResponseDto()
        {
            StartTime = q.StartTime,
            EndTime = q.EndTime,
            Status = q.Status,
            TotalHour = q.TotalHour,
            Id = q.Id,
        });

        return response;
    }
}
