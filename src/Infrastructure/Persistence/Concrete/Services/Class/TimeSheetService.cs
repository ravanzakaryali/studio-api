namespace Space.Infrastructure.Persistence.Services;

public class TimeSheetService : ITimeSheetService
{
    readonly IHolidayService _holidayService;
    public TimeSheetService(IHolidayService holidayService)
    {
        _holidayService = holidayService;
    }
    public async List<ClassTimeSheet> GenerateClassTimeSheet(
        int totalHour,
        List<CreateClassSessionDto> sessions,
        DateOnly startDate,
        Guid classId,
        Guid roomId)
    {
        List<ClassTimeSheet> timeSheets = new();
        List<DateOnly> holidayDates = await _holidayService.GetDatesAsync();
        DayOfWeek startDayOfWeek = startDate.DayOfWeek;
        int count = 0;

        while (totalHour > 0)
        {
            foreach (var session in sessions.OrderBy(c => c.DayOfWeek))
            {
                var daysToAdd = ((int)session.DayOfWeek - (int)startDayOfWeek + 7) % 7;
                int numSelectedDays = sessions.Count;

                int hour = (session.End - session.Start).Hours;

                DateOnly dateTime = startDate.AddDays(count * 7 + daysToAdd);
                ClassTimeSheet timeSheet = new()
                {
                    Category = session.Category,
                    ClassId = classId,
                    StartTime = session.Start,
                    EndTime = session.End,
                    RoomId = roomId,
                    TotalHours = hour,
                    Date = dateTime,
                    IsHoliday = false
                };
                if (holidayDates.Contains(dateTime))
                {
                    timeSheet.IsHoliday = true;
                }

                if (hour != 0)
                {
                    timeSheets.Add(timeSheet);
                    if (session.Category != ClassSessionCategory.Lab)
                        totalHour -= hour;
                    if (totalHour <= 0)
                        break;

                }
            }
            count++;
        }
        return timeSheets;
    }
}
