namespace Space.Application.Handlers;

public record GetClassCounterHourQuery(Guid Id) : IRequest<GetClassCounterHourResponseDto>;

internal class GetClassCounterHourQueryHandler : IRequestHandler<GetClassCounterHourQuery, GetClassCounterHourResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IClassRepository _classRepository;

    public GetClassCounterHourQueryHandler(
        IUnitOfWork unitOfWork,
        IClassRepository classRepository)
    {
        _unitOfWork = unitOfWork;
        _classRepository = classRepository;
    }

    public async Task<GetClassCounterHourResponseDto> Handle(GetClassCounterHourQuery request, CancellationToken cancellationToken)
    {
        Class? @class = await _classRepository.GetAsync(request.Id, tracking: false, "ClassSessions") ??
            throw new NotFoundException(nameof(Class), request.Id);
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
