namespace Space.Infrastructure.Persistence.Concrete;

internal class WorkerRepository : Repository<Worker>, IWorkerRepository
{
    public WorkerRepository(SpaceDbContext context) : base(context)
    {
    }
}
