using Space.Domain.Entities;

namespace Space.Application.Handlers;


public record GetClassCategoryHoursQuery(int Id, DateTime Date) : IRequest<IEnumerable<GetClassSessionCategoryHoursResponseDto>>;

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
        Class? @class = await _spaceDbContext.Classes
                    .Where(c => c.Id == request.Id)
                    .Include(c => c.Session)
                    .ThenInclude(c => c.Details)
                    .FirstOrDefaultAsync() ??
            throw new NotFoundException(nameof(Class), request.Id);

        DateOnly requestDate = DateOnly.FromDateTime(request.Date);

        if (@class.StartDate > requestDate || @class.EndDate < requestDate) throw new NotFoundException(
            "Class",
            $"Class {request.Id} is not available on {request.Date}"
        );

        bool isHoliday = await _spaceDbContext.Holidays
            .Where(c => c.StartDate <= requestDate && c.EndDate >= requestDate)
            .AnyAsync(cancellationToken: cancellationToken);

        if (isHoliday) throw new NotFoundException(
            "Today is one of the holidays. Please check the schedule."
        );


        DayOfWeek dayOfWeek = requestDate.DayOfWeek;

        List<SessionDetail> sessions = @class.Session.Details
            .Where(c => c.DayOfWeek == dayOfWeek)
            .ToList();

        if (sessions.Count == 0) throw new NotFoundException(
            "Session",
            $"Session not found for class {request.Id} on {request.Date}"
        );

        List<ClassTimeSheet> classTimeSheets = await _spaceDbContext.ClassTimeSheets
            .Where(c => c.Date == requestDate && c.ClassId == @class.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        IEnumerable<GetClassSessionCategoryHoursResponseDto> response = new List<GetClassSessionCategoryHoursResponseDto>();
        if (classTimeSheets.Count == 0)
        {
            response = sessions.Select(c => new GetClassSessionCategoryHoursResponseDto()
            {
                Category = c.Category,
                Status = ClassSessionStatus.Offline,
                Hour = c.TotalHours
            });
        }
        else
        {
            response = sessions
            .Select(c => new GetClassSessionCategoryHoursResponseDto()
            {
                Category = c.Category,
                Status = ClassSessionStatus.Offline,
                Hour = c.TotalHours
            });

        }
        return response;

    }
}
