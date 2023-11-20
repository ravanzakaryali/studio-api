namespace Space.Infrastructure.Persistence.Concrete.Services;

public class HolidayService : IHolidayService
{
    readonly ISpaceDbContext _spaceDbContext;

    public HolidayService(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<List<DateTime>> GetDatesAsync()
    {
        List<Holiday> holidays = await _spaceDbContext.Holidays.ToListAsync();

        List<DateTime> holidayDates = new();
        foreach (Holiday holiday in holidays)
        {
            for (DateOnly date = holiday.StartDate; date <= holiday.EndDate; date = date.AddDays(1))
            {
                holidayDates.Add(date.ToDateTime(new TimeOnly(0, 0)));
            }
        }
        return holidayDates;
    }
}
