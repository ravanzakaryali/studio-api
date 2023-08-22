namespace Space.Infrastructure.Persistence.Concrete;

internal class FileRepository : Repository<E.File>, IFileRepository
{
    public FileRepository(SpaceDbContext context) : base(context)
    {
    }
}
