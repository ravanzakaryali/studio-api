namespace Space.Infrastructure.Persistence.Concrete;

internal class RoomRepository : Repository<Room>, IRoomRepository
{
    public RoomRepository(SpaceDbContext context) : base(context) { }
}
