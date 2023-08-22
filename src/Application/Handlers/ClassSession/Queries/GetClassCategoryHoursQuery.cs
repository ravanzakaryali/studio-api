namespace Space.Application.Handlers;


public record GetClassCategoryHoursQuery(Guid Id, DateTime Date) : IRequest<IEnumerable<GetClassSessionCategoryHoursResponseDto>>;

internal class GetClassCategoryHoursQueryHandler : IRequestHandler<GetClassCategoryHoursQuery, IEnumerable<GetClassSessionCategoryHoursResponseDto>>
{
    readonly IUnitOfWork _unitOfWork;

    public GetClassCategoryHoursQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<GetClassSessionCategoryHoursResponseDto>> Handle(GetClassCategoryHoursQuery request, CancellationToken cancellationToken)
    {
        Class? @class = await _unitOfWork.ClassRepository.GetAsync(request.Id) 
            ?? throw new NotFoundException(nameof(Class),request.Id);
        IEnumerable<ClassSession> classSessions = await _unitOfWork.ClassSessionRepository.GetAllAsync(c => c.Date == request.Date && c.ClassId == @class.Id);

        IEnumerable<GetClassSessionCategoryHoursResponseDto> response = classSessions.Select(c => new GetClassSessionCategoryHoursResponseDto()
        {
            CategoryName = c.Category?.ToString()!,
            Hour = c.TotalHour
        });




        return response;
    }
}
