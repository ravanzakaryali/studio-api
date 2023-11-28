namespace Space.Application.Handlers;


public record GetClassCategoryHoursQuery(Guid Id, DateOnly Date) : IRequest<IEnumerable<GetClassSessionCategoryHoursResponseDto>>;

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

        List<ClassTimeSheet> classTimeSheets = await _spaceDbContext.ClassTimeSheets
            .Where(c => c.Date == request.Date && c.ClassId == @class.Id)
            .ToListAsync();

        IEnumerable<GetClassSessionCategoryHoursResponseDto> response = classTimeSheets
            .Select(c => new GetClassSessionCategoryHoursResponseDto()
            {
                CategoryName = c.Category?.ToString()!,
                Status = c.Status,
                Hour = c.TotalHours
            });

        return response;
    }
}
