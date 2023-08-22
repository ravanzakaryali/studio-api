namespace Space.Infrastructure.Persistence.Concrete;

internal class ClassRepository : Repository<Class>, IClassRepository
{
    private readonly SpaceDbContext _spaceDb;
    public ClassRepository(SpaceDbContext context) : base(context)
    {
        _spaceDb = context;
    }

}
