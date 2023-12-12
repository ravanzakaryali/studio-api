namespace Space.Application.Handlers;

public record GetSessionByClassQuery(int Id) : IRequest<IEnumerable<GetSessionDetailDto>>;

internal class GetSessionByClassQueryCommand : IRequestHandler<GetSessionByClassQuery, IEnumerable<GetSessionDetailDto>>
{
    readonly IMapper _mapper;
    readonly ISpaceDbContext _spaceDbContext;
    public GetSessionByClassQueryCommand(
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetSessionDetailDto>> Handle(GetSessionByClassQuery request, CancellationToken cancellationToken)
    {
        Class? @class = await _spaceDbContext.Classes
            .Include(c => c.Session)
            .ThenInclude(c => c.Details)
            .Where(c => c.Id == request.Id)
            .FirstOrDefaultAsync()
                ?? throw new NotFoundException(nameof(Class), request.Id);

        return @class.Session.Details.Select(c => new GetSessionDetailDto()
        {
            ClassName = @class.Name,
            DayOfWeek = c.DayOfWeek,
            EndTime = c.EndTime,
            StartTime = c.StartTime,
            Id = c.Id,
            TotalHours = c.TotalHours
        }).ToList().OrderBy(c => c.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)c.DayOfWeek);
    }
}
