namespace Space.Infrastructure.Persistence.Concrete;

internal class SessionDetailRepository : Repository<SessionDetail>, ISessionDetailRepository
{
    public SessionDetailRepository(SpaceDbContext context) : base(context)
    {
    }
}
