namespace Space.Application.Handlers;

public record GetClassSessionByClassQuery(Guid Id, DateTime Date) : IRequest<IEnumerable<GetClassSessionByClassResponseDto>>;
internal class GetClassSessionByClassQueryHandler : IRequestHandler<GetClassSessionByClassQuery, IEnumerable<GetClassSessionByClassResponseDto>>
{
    readonly IUnitOfWork _unitOfWork;

    public GetClassSessionByClassQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<GetClassSessionByClassResponseDto>> Handle(GetClassSessionByClassQuery request, CancellationToken cancellationToken)
    {
        Class @class = await _unitOfWork.ClassRepository.GetAsync(c => c.Id == request.Id, false, "ClassSessions")
            ?? throw new NotFoundException(nameof(Class), request.Id);
        IEnumerable<ClassSession> classSessions = @class.ClassSessions.Where(c => c.Date == request.Date);
        int totalHour = classSessions.Sum(c => c.TotalHour);
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
