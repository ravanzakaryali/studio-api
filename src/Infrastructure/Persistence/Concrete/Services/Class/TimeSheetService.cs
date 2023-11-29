namespace Space.Infrastructure.Persistence.Services;

public class TimeSheetService : ITimeSheetService
{
    readonly IHolidayService _holidayService;
    public TimeSheetService(IHolidayService holidayService)
    {
        _holidayService = holidayService;
    }
}
