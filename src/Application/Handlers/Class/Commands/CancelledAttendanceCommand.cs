using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Space.Application.Handlers.Commands;

public record CancelledAttendanceCommand(
        int ClassId, DateTime Date) : IRequest;
internal class CancelledAttendanceHandler : IRequestHandler<CancelledAttendanceCommand>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IHolidayService _holidayService;
    readonly IClassSessionService _classSessionService;

    public CancelledAttendanceHandler(
        ISpaceDbContext spaceDbContext,
        IHolidayService holidayService,
        IClassSessionService classSessionService)
    {
        _spaceDbContext = spaceDbContext;
        _holidayService = holidayService;
        _classSessionService = classSessionService;
    }

    public async Task Handle(CancelledAttendanceCommand request, CancellationToken cancellationToken)
    {
        Class? @class = await _spaceDbContext.Classes
                        .Include(c => c.Session)
                        .ThenInclude(c => c.Details)
                        .FirstOrDefaultAsync(c => c.Id == request.ClassId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Class), request.ClassId);

        DateOnly date = DateOnly.FromDateTime(request.Date);

        IEnumerable<ClassSession> classSessions = await _spaceDbContext.ClassSessions
            .Where(c => c.ClassId == request.ClassId && c.Date == date)
            .ToListAsync(cancellationToken: cancellationToken);

        if (!classSessions.Any())
        {
            throw new NotFoundException(nameof(ClassSession), request.ClassId);
        }
        IEnumerable<ClassTimeSheet> classTimeSheets = await _spaceDbContext.ClassTimeSheets
            .Where(c => c.ClassId == request.ClassId && c.Date == date)
            .ToListAsync(cancellationToken: cancellationToken);

        List<DateOnly> holidayDates = await _holidayService.GetDatesAsync();
        DateOnly classLastDate = await _classSessionService.GetLastDateAsync(@class.Id);




        foreach (ClassSession classSession in classSessions)
        {
            classSession.Status = ClassSessionStatus.Cancelled;
            DateOnly date2 = classLastDate.AddDays(1);
            DayOfWeek startDateDayOfWeek = date2.DayOfWeek;
            while (
                      !@class.Session.Details.Select(c => c.DayOfWeek).Any(c => date2.DayOfWeek == c)
                  )
            {
                date2 = date2.AddDays(1);
                startDateDayOfWeek = date2.DayOfWeek;
            }
            List<ClassSession> generateClassSessions = _classSessionService
                            .GenerateSessions(
                                classSession.TotalHours,
                                classSessions
                                    .Select(
                                        r =>
                                            new CreateClassSessionDto()
                                            {
                                                Category = r.Category,
                                                DayOfWeek = startDateDayOfWeek,
                                                End = classSession.EndTime,
                                                Start = classSession.StartTime
                                            }
                                    )
                                    .ToList(),
                                date2,
                                holidayDates,
                                @class.Id,
                                classSession.RoomId!.Value
                            );
            await _spaceDbContext.ClassSessions.AddRangeAsync(generateClassSessions, cancellationToken);
            @class.EndDate = date2;
        }



        _spaceDbContext.ClassTimeSheets.RemoveRange(classTimeSheets);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}
