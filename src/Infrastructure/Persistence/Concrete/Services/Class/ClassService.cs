namespace Space.Infrastructure.Persistence.Concrete.Services;

public class ClassService : IClassService
{
    private readonly ISpaceDbContext _dbContext;
    public ClassService(ISpaceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public (DateTime StartDate, DateTime EndDate) CalculateStartAndEndDate(Session session, Class @class, List<DateTime> holidayDates)
    {
        int count = 0;
        int totalHour = @class.Program.TotalHours;
        DateTime startDate = @class.StartDate ?? DateTime.Now;
        List<DateTime> sessionDates = new();
        while (totalHour > 0)
        {
            foreach (var sessionItem in session.Details.OrderBy(c => c.DayOfWeek))
            {
                var daysToAdd = ((int)sessionItem.DayOfWeek - (int)startDate.DayOfWeek + 7) % 7;
                int numSelectedDays = session.Details.Count;

                int hour = (sessionItem.EndTime - sessionItem.StartTime).Hours;


                DateTime dateTime = startDate.AddDays(count * 7 + daysToAdd);

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
