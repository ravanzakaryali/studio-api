namespace Space.Application.Handlers;


public record GetClassCategoryHoursQuery(Guid Id, DateTime Date) : IRequest<IEnumerable<GetClassSessionCategoryHoursResponseDto>>;

internal class GetClassCategoryHoursQueryHandler : IRequestHandler<GetClassCategoryHoursQuery, IEnumerable<GetClassSessionCategoryHoursResponseDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IClassSessionRepository _classSessionRepository;
    readonly IClassRepository _classRepository;

    public GetClassCategoryHoursQueryHandler(
        IUnitOfWork unitOfWork,
        IClassSessionRepository sessionRepository,
        IClassRepository classRepository)
    {
        _unitOfWork = unitOfWork;
        _classSessionRepository = sessionRepository;
        _classRepository = classRepository;
    }

    public async Task<IEnumerable<GetClassSessionCategoryHoursResponseDto>> Handle(GetClassCategoryHoursQuery request, CancellationToken cancellationToken)
    {
        Class? @class = await _classRepository.GetAsync(request.Id)
            ?? throw new NotFoundException(nameof(Class), request.Id);
        IEnumerable<ClassSession> classSessions = await _classSessionRepository.GetAllAsync(c => c.Date == request.Date && c.ClassId == @class.Id);

        IEnumerable<GetClassSessionCategoryHoursResponseDto> response = classSessions.Select(c => new GetClassSessionCategoryHoursResponseDto()
        {
            CategoryName = c.Category?.ToString()!,
            Status = c.Status,
            Hour = c.TotalHour
        });

        return response;
    }
}
