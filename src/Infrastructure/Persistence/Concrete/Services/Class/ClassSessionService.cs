namespace Space.Infrastructure.Persistence.Services;

public class ClassSessionService : IClassSessionService
{
    private readonly IHolidayService _holidayService;
    private readonly ISpaceDbContext _context;

    public ClassSessionService(
        ISpaceDbContext context,
        IHolidayService holidayService)
    {
        _context = context;
        _holidayService = holidayService;
    }
    public List<ClassSession> GenerateSessions(
                                int totalHours,
                                List<CreateClassSessionDto> sessions,
                                DateOnly startDate,
                                List<DateOnly> holidayDates,
                                int classId,
                                int roomId)
    {

        List<ClassSession> returnClassSessions = new();
        DayOfWeek startDayOfWeek = startDate.DayOfWeek;
        int count = 0;
        while (totalHours > 0)
        {
            foreach (var session in sessions.OrderBy(c => c.DayOfWeek))
            {
                var daysToAdd = ((int)session.DayOfWeek - (int)startDayOfWeek + 7) % 7;
                int numSelectedDays = sessions.Count;

                int hour = (session.End - session.Start).Hours;

                DateOnly dateTime = startDate.AddDays(count * 7 + daysToAdd);
                ClassSession classSession = new()
                {
                    Category = session.Category,
                    ClassId = classId,
                    StartTime = session.Start,
                    EndTime = session.End,
                    TotalHours = hour,
                    Date = dateTime,
                    IsHoliday = false
                };
                if (holidayDates.Contains(dateTime))
                {
                    classSession.IsHoliday = true;
                }

                if (hour != 0)
                {
                    returnClassSessions.Add(classSession);
                    if (session.Category != ClassSessionCategory.Lab)
                        totalHours -= hour;
                    if (totalHours <= 0)
                        break;

                }
            }
            count++;

        }
        return returnClassSessions;
    }
    public List<ClassSession> GenerateSessions(
                                DateOnly startDate,
                                List<CreateClassSessionDto> sessions,
                                DateOnly endDate,
                                List<DateOnly> holidayDates,
                                int classId,
                                int roomId)
    {

        List<ClassSession> returnClassSessions = new();
        DayOfWeek startDayOfWeek = startDate.DayOfWeek;
        int count = 0;
        while (returnClassSessions.Any(c => c.Date == endDate))
        {
            foreach (var session in sessions.OrderBy(c => c.DayOfWeek))
            {
                var daysToAdd = ((int)session.DayOfWeek - (int)startDayOfWeek + 7) % 7;
                int numSelectedDays = sessions.Count;

                int hour = (session.End - session.Start).Hours;

                DateOnly dateTime = startDate.AddDays(count * 7 + daysToAdd);

                if (holidayDates.Contains(dateTime))
                {
                    continue;
                }

                if (hour != 0)
                {
                    returnClassSessions.Add(new ClassSession()
                    {
                        Category = session.Category,
                        ClassId = classId,
                        StartTime = session.Start,
                        EndTime = session.End,
                        RoomId = roomId,
                        TotalHours = hour,
                        Date = dateTime
                    });
                    if (dateTime == endDate)
                        break;

                }
            }
            count++;

        }
        return returnClassSessions;
    }

    public async Task<DateOnly> GetLastDateAsync(int classId)
    {
        DateOnly lastDate = await _context.ClassSessions
                                                        .Where(c => c.ClassId == classId)
                                                        .Select(c => c.Date)
                                                        .OrderByDescending(c => c)
                                                        .FirstOrDefaultAsync();
        return lastDate;
    }
}
