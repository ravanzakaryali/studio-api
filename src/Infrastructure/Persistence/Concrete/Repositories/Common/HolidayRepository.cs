using Space.Application.Abstractions;

namespace Space.Infrastructure.Persistence.Concrete;

internal class HolidayRepository : Repository<Holiday>, IHolidayRepository
{
    readonly ISpaceDbContext _context;
    public HolidayRepository(SpaceDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<DateTime>> GetDatesAsync()
    {
        List<Holiday> holidays = await _context.Holidays.ToListAsync();

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
