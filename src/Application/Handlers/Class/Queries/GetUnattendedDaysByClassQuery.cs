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
            .Include(c => c.Session)
            .ThenInclude(s => s.Details)
            .Where(c => c.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                throw new NotFoundException(nameof(Class), request.Id);

        DateOnly dateNow = DateOnly.FromDateTime(DateTime.Now);

        DateOnly startDateClass = @class.StartDate;
        DateOnly endDateClass = @class.EndDate.HasValue ? @class.EndDate.Value : dateNow;

        List<ClassTimeSheet> classTimeSheets = await _spaceDbContext.ClassTimeSheets
            .Where(cts => cts.ClassId == @class.Id && cts.Date < dateNow)
            .ToListAsync(cancellationToken);

        List<DateOnly> relevantDates = new();
        
        for (DateOnly date = startDateClass; date <= endDateClass; date = date.AddDays(1))
        {
            DayOfWeek dayOfWeek = date.DayOfWeek;
            bool hasSession = @class.Session.Details.Any(sd => sd.DayOfWeek == dayOfWeek);
            if (hasSession)
            {
                relevantDates.Add(date);
            }
        }

        List<DateOnly> unattendedDates = relevantDates
            .Where(date => !classTimeSheets.Any(cts => cts.Date == date) && date < dateNow)
            .ToList();

        return unattendedDates.Select(date => new GetUnAttendedByClassDto()
        {
            Date = date
        })
        .OrderByDescending(c => c.Date);
    }
}
