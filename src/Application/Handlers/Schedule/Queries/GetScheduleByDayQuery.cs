using System;

namespace Space.Application.Handlers
{
    public record GetScheduleByDayQuery() : IRequest<IEnumerable<GetScheduleByDayResponseDto>>;

    internal class GetScheduleByDayQueryHandler : IRequestHandler<GetScheduleByDayQuery, IEnumerable<GetScheduleByDayResponseDto>>
    {

        readonly IUnitOfWork _unitOfWork;


        public GetScheduleByDayQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;

        }


        public async Task<IEnumerable<GetScheduleByDayResponseDto>> Handle(GetScheduleByDayQuery request, CancellationToken cancellationToken)
        {

            var roomSchedule = new List<RoomSchedule>();


            var roomScheduleQuery = await _unitOfWork.RoomScheduleRepository
                              .GetAllAsync(q => q.DayOfMonth == DateTime.Now.Month && q.Year == DateTime.Now.Year, tracking: false, "Class.Program");

            roomSchedule = roomScheduleQuery.ToList();
            roomSchedule = roomScheduleQuery.ToList();


            var response = new List<GetScheduleByDayResponseDto>();


            var rooms = await _unitOfWork.RoomRepository.GetAllAsync();




            //7 günlük calendar

            for (int i = -7; i <= 100; i++)
            {
                var data = new GetScheduleByDayResponseDto();
                data.Date = DateTime.Now.AddDays(i);

                var calendars = new List<CalendarDto>();

                foreach (var item in rooms)
                {

                    var calendar = new CalendarDto();

                    calendar.RoomName = item.Name;
                    calendar.RoomCapacity = item.Capacity ?? 0;

                    var sessionDtoForScheduleList = new List<SessionDtoForSchedule>();

                    var sessionsByRoom = roomSchedule.Where(q => q.RoomId == item.Id && q.GeneralDate.DayOfYear == (DateTime.Now.DayOfYear + i) && (q.GeneralDate.Year == DateTime.Now.Year));

                    foreach (var session in sessionsByRoom)
                    {
                        var sessionDtoForSchedule = new SessionDtoForSchedule();
                        sessionDtoForSchedule.StartTime = session.StartTime;
                        sessionDtoForSchedule.Endtime = session.EndTime;
                        sessionDtoForSchedule.ClassName = session.Class?.Name;
                        sessionDtoForSchedule.ClassColor = session.Class?.Program?.Color;
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
            // Calculate the date of the first day of the given year
            DateTime startDate = new DateTime(year, 1, 1);

            // Calculate the date of the first Monday of the first week of the year
            int daysUntilFirstMonday = ((int)startDate.DayOfWeek - 1 + 7) % 7;
            DateTime firstMondayOfYear = startDate.AddDays(daysUntilFirstMonday);

            // Calculate the date of the first Monday of the given week
            DateTime firstMondayOfWeek = firstMondayOfYear.AddDays((weekNumber - 1) * 7);

            return firstMondayOfWeek;
        }
    }



}

