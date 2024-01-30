namespace Space.Infrastructure.Persistence.Concrete.Services;

public class HolidayService : IHolidayService
{
    readonly ISpaceDbContext _spaceDbContext;

    public HolidayService(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<List<DateOnly>> GetDatesAsync()
    {
        List<Holiday> holidays = await _spaceDbContext.Holidays.ToListAsync();

        List<DateOnly> holidayDates = new();
        foreach (Holiday holiday in holidays)
        {
            for (DateOnly date = holiday.StartDate; date <= holiday.EndDate; date = date.AddDays(1))
            {
                holidayDates.Add(date);
        }
    }
        return holidayDates;
    }
}
