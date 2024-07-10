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
            int numSelectedDays = sessions.Count;
            DateOnly dateTime = startDate.AddDays(count);
            while (true)
            {
                if (sessions.Any(c => (int)c.DayOfWeek == (int)dateTime.DayOfWeek))
                {
                    if (dateTime <= startDate)
                    {
                        dateTime = dateTime.AddDays(1);
                        count++;
                        continue;
                    }
                    if (holidayDates.Contains(dateTime))
                    {
                        dateTime = dateTime.AddDays(1);
                        count++;
                        continue;
                    }
                    break;
                }
                dateTime = dateTime.AddDays(1);
                count++;
            }


            CreateClassSessionDto session = sessions.First(c => (int)c.DayOfWeek == (int)dateTime.DayOfWeek);
            int hour = (session.End - session.Start).Hours;



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
                totalHours -= hour;
                if (totalHours <= 0)
                    break;
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
        while (!returnClassSessions.Any(c => c.Date >= endDate))
        {
            int numSelectedDays = sessions.Count;
            DateOnly dateTime = startDate.AddDays(count);
            while (true)
            {
                if (sessions.Any(c => (int)c.DayOfWeek == (int)dateTime.DayOfWeek))
                {
                    if (dateTime <= startDate)
                    {
                        count++;
                        dateTime = dateTime.AddDays(1);
                        continue;
                    }
                    if (holidayDates.Contains(dateTime))
                    {
                        count++;
                        dateTime = dateTime.AddDays(1);
                        continue;
                    }
                    break;
                }
                count++;
                dateTime = dateTime.AddDays(1);
            }
            CreateClassSessionDto session = sessions.First(c => (int)c.DayOfWeek == (int)dateTime.DayOfWeek);
            int hour = (session.End - session.Start).Hours;
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
