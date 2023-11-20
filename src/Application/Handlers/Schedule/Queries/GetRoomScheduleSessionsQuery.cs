namespace Space.Application.Handlers;
public record GetRoomScheduleSessionsQuery() : IRequest<IEnumerable<GetRoomScheduleSessionsResponseDto>>;


internal class GetRoomScheduleSessionsQueryHandler : IRequestHandler<GetRoomScheduleSessionsQuery, IEnumerable<GetRoomScheduleSessionsResponseDto>>
{
    readonly ISpaceDbContext _spaceDbContext;
    public GetRoomScheduleSessionsQueryHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetRoomScheduleSessionsResponseDto>> Handle(GetRoomScheduleSessionsQuery request, CancellationToken cancellationToken)
    {

        IEnumerable<RoomSchedule> roomScheduleQuery = await _spaceDbContext.RoomSchedules
            .Include(c => c.Class)
            .ThenInclude(c => c.Session)
            .Where(q => q.Year == 2023)
            .ToListAsync();

        IEnumerable<Room> rooms = await _spaceDbContext.Rooms.ToListAsync();


        List<GetRoomScheduleSessionsResponseDto> response = new();

        foreach (Room room in rooms)
        {
            GetRoomScheduleSessionsResponseDto model = new()
            {
                RoomName = room.Name
            };
            List<WeekScheduleSessions> weekScheduleSessions = new();

            for (int i = 0; i <= 52; i++)
            {
                WeekScheduleSessions weekScheduleModel = new()
                {
                    WeekOfYear = i + 1
                };

                var getRoomScheduleSessionsByWeekDtos = new List<GetRoomScheduleSessionsByWeekDto>();
                var roomSchedules = roomScheduleQuery.Where(q => (q.GeneralDate.DayOfYear / 7) == i && q.RoomId == room.Id);
                foreach (var roomSchedule in roomSchedules)
                {
                    GetRoomScheduleSessionsByWeekDto roomScheduleSessionsByWeekDto = new()
                    {
                        ClassName = roomSchedule.Class?.Name,
                        SessionName = roomSchedule.Class?.Session?.Name
                    };

                    if (!getRoomScheduleSessionsByWeekDtos.Any(q => q.ClassName == roomSchedule.Class?.Name && q.SessionName == roomSchedule.Class?.Session?.Name))
                        getRoomScheduleSessionsByWeekDtos.Add(roomScheduleSessionsByWeekDto);
                }
                weekScheduleModel.GetRoomScheduleSessionsByWeeks = getRoomScheduleSessionsByWeekDtos;
                weekScheduleSessions.Add(weekScheduleModel);
            }
            model.WeekScheduleSessions = weekScheduleSessions;
            response.Add(model);
        }

        return response;
    }
}



