namespace Space.Infrastructure.Persistence.Concrete.Services;

public class ClassService : IClassService
{
    private readonly ISpaceDbContext _dbContext;
    public ClassService(ISpaceDbContext dbContext)
    {
        _dbContext = dbContext;
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
}
