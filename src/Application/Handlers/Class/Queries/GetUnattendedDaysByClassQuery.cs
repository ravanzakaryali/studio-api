
namespace Space.Application.Handlers;

public class GetUnattendedDaysByClassQuery : IRequest<IEnumerable<GetUnAttendedByClassDto>>
{
    public int Id { get; set; }
}
internal class GetUnattendedDaysByClassHandler : IRequestHandler<GetUnattendedDaysByClassQuery, IEnumerable<GetUnAttendedByClassDto>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetUnattendedDaysByClassHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetUnAttendedByClassDto>> Handle(GetUnattendedDaysByClassQuery request, CancellationToken cancellationToken)
    {
        Class @class = await _spaceDbContext.Classes
            .Where(c => c.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                throw new NotFoundException(nameof(Class), request.Id);

        DateOnly dateNow = DateOnly.FromDateTime(DateTime.Now);

        List<DateOnly> classSessionDates = await _spaceDbContext.ClassSessions
            .Include(c => c.ClassTimeSheet)
            .Where(c => c.ClassId == @class.Id && c.Date < dateNow && c.ClassTimeSheetId == null && c.Status != ClassSessionStatus.Cancelled)
            .Select(c => c.Date)
            .ToListAsync(cancellationToken: cancellationToken);

        return classSessionDates.Select(c => new GetUnAttendedByClassDto()
        {
            Date = c
        }).OrderByDescending(c => c.Date);
    }
}
