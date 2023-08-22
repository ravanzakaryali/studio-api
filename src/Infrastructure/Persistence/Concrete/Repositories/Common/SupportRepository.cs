namespace Space.Infrastructure.Persistence.Concrete;

internal class SupportRepository : Repository<Support>, ISupportRepository
{
    public SupportRepository(SpaceDbContext context) : base(context)
    {
    }
}
