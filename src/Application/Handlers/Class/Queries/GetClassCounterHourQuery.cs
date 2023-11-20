namespace Space.Application.Handlers;

public record GetClassCounterHourQuery(Guid Id) : IRequest<GetClassCounterHourResponseDto>;

internal class GetClassCounterHourQueryHandler : IRequestHandler<GetClassCounterHourQuery, GetClassCounterHourResponseDto>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetClassCounterHourQueryHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<GetClassCounterHourResponseDto> Handle(GetClassCounterHourQuery request, CancellationToken cancellationToken)
    {
        Class? @class = await _spaceDbContext.Classes
            .Where(c => c.Id == request.Id)
            .Include(c => c.ClassSessions)
            .FirstOrDefaultAsync()
                ?? throw new NotFoundException(nameof(Class), request.Id);
        return new GetClassCounterHourResponseDto()
        {
            TotalHour = @class.ClassSessions.Where(c => c.Category != ClassSessionCategory.Lab).Sum(c => c.TotalHour),
            Hour = @class.ClassSessions.Where(c =>
            c.Status != ClassSessionStatus.Cancelled &&
            c.Category != ClassSessionCategory.Lab &&
            c.Date <= DateTime.UtcNow).Sum(c => c.TotalHour)
        };
    }
}
