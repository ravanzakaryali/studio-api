namespace Space.Application.Handlers;

public class GetRoomPlaningQuery : IRequest<IEnumerable<GetRoomPlaningDto>>
{
    public GetRoomPlaningQuery()
    {
        SessionIds = new List<int>();
        RoomIds = new List<int>();
    }
    public int Year { get; set; }
    public List<int> SessionIds { get; set; }
    public List<int> RoomIds { get; set; }
}
internal class GetRoomPlaningQueryHandler : IRequestHandler<GetRoomPlaningQuery, IEnumerable<GetRoomPlaningDto>>
{
    readonly ISpaceDbContext _dbContext;
    public GetRoomPlaningQueryHandler(ISpaceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<GetRoomPlaningDto>> Handle(GetRoomPlaningQuery request, CancellationToken cancellationToken)
    {
        List<Room> rooms = await _dbContext.Rooms.ToListAsync();
        List<Session> sessions = await _dbContext.Sessions.ToListAsync();

        if (request.RoomIds.Count > 0)
        {
            rooms = rooms.Where(r => request.RoomIds.Contains(r.Id)).ToList();
        }

        List<Class> allClasses = await _dbContext.Classes.Include(c => c.Studies).Include(c => c.Session).ToListAsync();
        allClasses = allClasses.Where(c => c.StartDate.Year == request.Year || c.EndDate?.Year == request.Year).ToList();

        List<GetRoomPlaningDto> respnose = new();

        foreach (Room room in rooms)
        {
            GetRoomPlaningDto responseRoom = new()
            {
                Name = room.Name,
                Id = room.Id,
            };
            foreach (Session session in sessions.DistinctBy(s => s.No))
            {
                GetSessionClassResponseDto responseSessions = new()
                {
                    Id = session.Id,
                    Name = session.No.ToString() ?? "1",
                };

                List<Class> classes = allClasses.Where(c => c.RoomId == room.Id && c.Session.No == session.No).ToList();

                if (request.SessionIds.Count > 0)
                {
                    classes = classes.Where(c => request.SessionIds.Contains(c.SessionId)).ToList();
                }
                responseSessions.Classes.AddRange(classes.Select(cl => new GetClassDetailDto()
                {
                    Id = cl.Id,
                    EndDate = cl.EndDate,
                    StartDate = cl.StartDate,
                    Name = cl.Name,
                    StudentCount = cl.Studies.Count,
                }));
                responseRoom.Sessions.Add(responseSessions);
            }
            respnose.Add(responseRoom);
        }



        return respnose;
    }
}