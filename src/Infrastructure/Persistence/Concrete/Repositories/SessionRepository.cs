namespace Space.Infrastructure.Persistence.Concrete;

internal class SessionRepository : Repository<Session>, ISessionRepository
{
    public SessionRepository(SpaceDbContext context) : base(context)
    {
    }
}
