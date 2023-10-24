namespace Space.Application.Handlers;


public record GetSessionByClassQuery(Guid Id) : IRequest<IEnumerable<GetSessionDetailDto>>;

internal class GetSessionByClassQueryCommand : IRequestHandler<GetSessionByClassQuery, IEnumerable<GetSessionDetailDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly IClassRepository _classRepository;
    public GetSessionByClassQueryCommand(IUnitOfWork unitOfWork, IMapper mapper, IClassRepository classRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _classRepository = classRepository;
    }

    public async Task<IEnumerable<GetSessionDetailDto>> Handle(GetSessionByClassQuery request, CancellationToken cancellationToken)
    {
        Class? @class = await _classRepository.GetAsync(r => r.Id == request.Id, tracking: false, "Session.Details")
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
