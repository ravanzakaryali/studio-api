namespace Space.Application.Handlers;

public record GetClassSessionsByDateQuery(int Id, DateOnly Date) : IRequest<IEnumerable<GetClassSessionsByDateResponseDto>>;


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
        List<ClassTimeSheet> classTimeSheets = await _spaceDbContext.ClassTimeSheets
            .Where(c => c.Date == request.Date &&
                      c.ClassId == request.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        //Todo: total hour name changed

        IEnumerable<GetClassSessionsByDateResponseDto> response = classTimeSheets.Select(q => new GetClassSessionsByDateResponseDto()
        {
            StartTime = q.StartTime,
            EndTime = q.EndTime ?? null,
            Status = q.Status,
            TotalHour = q.TotalHours,
            Id = q.Id,
        });

        return response;
    }
}
