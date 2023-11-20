namespace Space.Application.Handlers;


public record GetClassCategoryHoursQuery(Guid Id, DateTime Date) : IRequest<IEnumerable<GetClassSessionCategoryHoursResponseDto>>;

internal class GetClassCategoryHoursQueryHandler : IRequestHandler<GetClassCategoryHoursQuery, IEnumerable<GetClassSessionCategoryHoursResponseDto>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetClassCategoryHoursQueryHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetClassSessionCategoryHoursResponseDto>> Handle(GetClassCategoryHoursQuery request, CancellationToken cancellationToken)
    {
        Class? @class = await _spaceDbContext.Classes.FindAsync(request.Id) ??
            throw new NotFoundException(nameof(Class), request.Id);

        List<ClassSession> classSessions = await _spaceDbContext.ClassSessions
            .Where(c => c.Date == request.Date && c.ClassId == @class.Id)
            .ToListAsync();

        IEnumerable<GetClassSessionCategoryHoursResponseDto> response = classSessions.Select(c => new GetClassSessionCategoryHoursResponseDto()
        {
            CategoryName = c.Category?.ToString()!,
            Status = c.Status,
            Hour = c.TotalHour
        });

        return response;
    }
}
