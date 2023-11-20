namespace Space.Application.Handlers;

public record GetScheduleByDayQuery() : IRequest<IEnumerable<GetScheduleByDayResponseDto>>;

internal class GetScheduleByDayQueryHandler : IRequestHandler<GetScheduleByDayQuery, IEnumerable<GetScheduleByDayResponseDto>>
{
    readonly ISpaceDbContext _spaceDbContext;
    public GetScheduleByDayQueryHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetScheduleByDayResponseDto>> Handle(GetScheduleByDayQuery request, CancellationToken cancellationToken)
    {

        var roomSchedule = new List<RoomSchedule>();
        List<RoomSchedule> roomScheduleQuery = await _spaceDbContext.RoomSchedules
            .Include(c => c.Class)
            .ThenInclude(c => c.Program)
            .Where(q => q.DayOfMonth == DateTime.Now.Month && q.Year == DateTime.Now.Year)
            .ToListAsync();

        roomSchedule = roomScheduleQuery.ToList();
        roomSchedule = roomScheduleQuery.ToList();

        var response = new List<GetScheduleByDayResponseDto>();

        var rooms = await _spaceDbContext.Rooms.ToListAsync();

        //7 günlük calendar
        for (int i = -7; i <= 100; i++)
        {
            var data = new GetScheduleByDayResponseDto
            {
                Date = DateTime.Now.AddDays(i)
            };

            var calendars = new List<CalendarDto>();

            foreach (var item in rooms)
            {

                var calendar = new CalendarDto
                {
                    RoomName = item.Name,
                    RoomCapacity = item.Capacity ?? 0
                };

                var sessionDtoForScheduleList = new List<SessionDtoForSchedule>();

                var sessionsByRoom = roomSchedule.Where(q => q.RoomId == item.Id && q.GeneralDate.DayOfYear == (DateTime.Now.DayOfYear + i) && (q.GeneralDate.Year == DateTime.Now.Year));

                foreach (var session in sessionsByRoom)
                {
                    if (session.Class == null) break;
                    var sessionDtoForSchedule = new SessionDtoForSchedule
                    {
                        StartTime = session.StartTime,
                        Endtime = session.EndTime,
                        ClassName = session.Class.Name,
                        ClassColor = session.Class.Program?.Color
                    };
                    sessionDtoForScheduleList.Add(sessionDtoForSchedule);

                }

                calendar.Sessions = sessionDtoForScheduleList.OrderBy(q => q.StartTime);
                calendars.Add(calendar);
                data.Calendar = calendars;
            }

            response.Add(data);
        }



        return response;
    }

    public static DateTime GetFirstMondayOfWeek(int weekNumber, int year)
    {
        DateTime startDate = new(year, 1, 1);

        int daysUntilFirstMonday = ((int)startDate.DayOfWeek - 1 + 7) % 7;
        DateTime firstMondayOfYear = startDate.AddDays(daysUntilFirstMonday);

        // Calculate the date of the first Monday of the given week
        DateTime firstMondayOfWeek = firstMondayOfYear.AddDays((weekNumber - 1) * 7);

        return firstMondayOfWeek;
    }
}

