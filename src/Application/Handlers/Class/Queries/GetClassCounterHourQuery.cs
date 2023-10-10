namespace Space.Application.Handlers;

public record GetClassCounterHourQuery(Guid Id) : IRequest<GetClassCounterHourResponseDto>;

internal class GetClassCounterHourQueryHandler : IRequestHandler<GetClassCounterHourQuery, GetClassCounterHourResponseDto>
{
    readonly IUnitOfWork _unitOfWork;

    public GetClassCounterHourQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<GetClassCounterHourResponseDto> Handle(GetClassCounterHourQuery request, CancellationToken cancellationToken)
    {
        Class? @class = await _unitOfWork.ClassRepository.GetAsync(request.Id, tracking: false, "ClassSessions") ??
            throw new NotFoundException(nameof(Class), request.Id);
        return new GetClassCounterHourResponseDto()
        {
            TotalHour = @class.ClassSessions.Where(c => c.Category != ClassSessionCategory.Lab).Sum(c => c.TotalHour),
            Hour = @class.ClassSessions.Where(c =>
            c.Status != null &&
            c.Status != ClassSessionStatus.Cancelled &&
            c.Category != ClassSessionCategory.Lab &&
            c.Date <= DateTime.UtcNow).Sum(c => c.TotalHour)
        };
    }
}
