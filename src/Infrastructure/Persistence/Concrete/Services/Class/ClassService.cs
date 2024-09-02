
namespace Space.Infrastructure.Persistence.Concrete.Services;

public class ClassService : IClassService
{
    readonly ISpaceDbContext _dbContext;
    readonly IHolidayService _holidayService;
    public ClassService(ISpaceDbContext dbContext, IHolidayService holidayService)
    {
        _dbContext = dbContext;
        _holidayService = holidayService;
    }

    public (DateOnly StartDate, DateOnly EndDate) CalculateStartAndEndDate(Session session, Class @class, List<DateOnly> holidayDates)
    {
        int count = 0;
        int totalHour = @class.Program.TotalHours;
        DateOnly startDate = @class.StartDate;
        List<DateOnly> sessionDates = new();
        while (totalHour > 0)
        {
            foreach (var sessionItem in session.Details.OrderBy(c => c.DayOfWeek))
            {
                var daysToAdd = ((int)sessionItem.DayOfWeek - (int)startDate.DayOfWeek + 7) % 7;
                int numSelectedDays = session.Details.Count;

                int hour = (sessionItem.EndTime - sessionItem.StartTime).Hours;


                DateOnly dateTime = startDate.AddDays(count * 7 + daysToAdd);

                if (holidayDates.Contains(dateTime))
                {
                    continue;
                }

                if (hour != 0)
                {
                    sessionDates.Add(dateTime);
                    totalHour -= hour;
                    if (totalHour <= 0)
                        break;

                }
            }
            count++;
        }
        sessionDates = sessionDates.OrderBy(c => c).ToList();
        return (sessionDates.First(), sessionDates.Last());
    }

    public Task CheckClassAvailabilityAsync(Class @class, DateOnly date)
    {
        throw new NotImplementedException();
    }

    public async Task<DateOnly> EndDateCalculationAsync(Class @class)
    {
        if (@class.Program is null) throw new NullReferenceException(nameof(@class.Program));
        if (@class.Session is null) throw new NullReferenceException(nameof(@class.Session));
        if (@class.Session.Details is null) throw new NullReferenceException(nameof(@class.Session.Details));
        List<DateOnly> holidayDates = await _holidayService.GetDatesAsync();
        ICollection<SessionDetail> sessions = @class.Session.Details;
        DateOnly startDate = @class.StartDate;
        int totalProgramHours = @class.Program.TotalHours;

        Dictionary<DayOfWeek, int> sessionHours = new Dictionary<DayOfWeek, int>();

        foreach (var session in sessions)
        {
            if (sessionHours.ContainsKey(session.DayOfWeek))
            {
                sessionHours[session.DayOfWeek] += session.TotalHours;
            }
            else
            {
                sessionHours[session.DayOfWeek] = session.TotalHours;
            }
        }

        DateOnly currentDate = startDate;
        int remainingHours = totalProgramHours;

        while (remainingHours > 0)
        {
            DayOfWeek currentDay = currentDate.DayOfWeek;

            if (!holidayDates.Contains(currentDate) && sessionHours.ContainsKey(currentDay))
            {
                remainingHours -= sessionHours[currentDay];
            }

            if (remainingHours > 0)
            {
                currentDate = currentDate.AddDays(1);
            }
        }

        return currentDate;
    }

    public async Task<string> GenerateClassName(Class @class)
    {
        string responseName = string.Empty;

        responseName += @class.Program?.ShortName;
        responseName += @class.Session?.No;

        string lastNumber = string.Empty;
        Class? lastClass = await _dbContext.Classes
                                .Where(c => c.SessionId == @class.SessionId && c.ProgramId == @class.ProgramId)
                                .OrderByDescending(c => c.CreatedDate)
                                .FirstOrDefaultAsync();
        if (lastClass is null)
        {
            lastNumber = "01";
        }
        else
        {
            string lastClassName = lastClass.Name;
            lastNumber = lastClassName.Substring(lastClassName.Length - 2);
            int lastNumberInt = int.Parse(lastNumber);
            lastNumberInt++;
            lastNumber = lastNumberInt.ToString().PadLeft(2, '0');
        }

        responseName += lastNumber;

        return responseName;
    }
}
