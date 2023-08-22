namespace Space.Infrastructure.Persistence;


internal class RoomScheduleRepository : Repository<RoomSchedule>, IRoomScheduleRepository
    {

        public RoomScheduleRepository(SpaceDbContext context) : base(context) { }

    }



