namespace Space.Infrastructure.Persistence;

internal class ClassModuleWorkerRepository : Repository<ClassModulesWorker>, IClassModulesWorkerRepository
{
    private readonly SpaceDbContext _spaceDbContext;
    public ClassModuleWorkerRepository(SpaceDbContext context) : base(context)
    {
        _spaceDbContext = context;
    }
    public async Task<bool> IsWorkerExist(Guid workerId, List<Guid> moduleIds)
        => await _spaceDbContext.ClassModulesWorkers.AnyAsync(m => m.WorkerId == workerId && moduleIds.Contains(m.ModuleId));
}