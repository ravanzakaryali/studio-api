using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Space.Application.Enums;

namespace Space.Application.Handlers;

public class GetAttendanceRateByClassQuery : IRequest<IEnumerable<GetAttendanceRateByClassDto>>
{
    public int Id { get; set; }
    public MonthOfYear MonthOfYear { get; set; }
    public int Year { get; set; }
}

internal class GetAttendanceRateByClassHandler : IRequestHandler<GetAttendanceRateByClassQuery, IEnumerable<GetAttendanceRateByClassDto>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetAttendanceRateByClassHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetAttendanceRateByClassDto>> Handle(GetAttendanceRateByClassQuery request, CancellationToken cancellationToken)
    {
        Class? @class = await _spaceDbContext.Classes
            .Where(c => c.Id == request.Id)
            .Include(c => c.Session)
            .ThenInclude(c => c.Details)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                throw new NotFoundException(nameof(Class), request.Id);

        int month = (int)request.MonthOfYear;
        DateOnly dateNow = DateOnly.FromDateTime(DateTime.Now);

        List<ClassTimeSheet> classTimeSheets = await _spaceDbContext.ClassTimeSheets
            .Where(cts => cts.ClassId == request.Id)
            .Include(c => c.Attendances)
            .ToListAsync(cancellationToken);

        classTimeSheets = classTimeSheets
            .Where(cts => cts.Date.Month == month && cts.Date.Year == request.Year)
            .ToList();

        List<GetAttendanceRateByClassDto> response = new();

        ICollection<SessionDetail> sessionDetails = @class.Session.Details;
        int daysInMonth = DateTime.DaysInMonth(request.Year, month);

        DateOnly startDate = @class.StartDate;
        DateOnly? endDate = @class.EndDate;
        DateOnly firstDayOfMonth = new(request.Year, month, 1);
        DateOnly lastDayOfMonth = new(request.Year, month, daysInMonth);

        DateOnly startProcessingDate = startDate > firstDayOfMonth ? startDate : firstDayOfMonth;
        DateOnly endProcessingDate = endDate.HasValue && endDate < lastDayOfMonth ? endDate.Value : lastDayOfMonth;

        for (DateOnly date = startProcessingDate; date <= endProcessingDate; date = date.AddDays(1))
        {
            DayOfWeek dayOfWeek = date.DayOfWeek;

            IEnumerable<GetAttendanceRateByClassDto> matchingSessions = sessionDetails
                .Where(sd => sd.DayOfWeek == dayOfWeek)
                .Select(sd => new GetAttendanceRateByClassDto
                {
                    Date = date,
                    Status = null,
                    AttendingStudentsCount = null,
                    TotalStudentsCount = null
                });

            response.AddRange(matchingSessions);
        }

        foreach (ClassTimeSheet classTimeSheet in classTimeSheets)
        {
            GetAttendanceRateByClassDto? matchingSession = response.FirstOrDefault(r => r.Date == classTimeSheet.Date);
            if (matchingSession is not null)
            {
                matchingSession.Status = classTimeSheet.Status;
                if (classTimeSheet.Status != ClassSessionStatus.Cancelled)
                {
                    matchingSession.AttendingStudentsCount = classTimeSheet?.Attendances.Where(c => c.TotalAttendanceHours != 0).Count();
                    matchingSession.TotalStudentsCount = classTimeSheet?.Attendances.Count;
                }
            }
        }

        return response.DistinctBy(r => r.Date);
    }
}
