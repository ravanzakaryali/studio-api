namespace Space.Infrastructure.Persistence;

internal class ClassSessionRepository : Repository<ClassSession>, IClassSessionRepository
{
    public ClassSessionRepository(SpaceDbContext context) : base(context)
    {
    }
}
