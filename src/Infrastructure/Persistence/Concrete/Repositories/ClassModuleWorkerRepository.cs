namespace Space.Infrastructure.Persistence;

internal class ClassModuleWorkerRepository : Repository<ClassModulesWorker>, IClassModulesWorkerRepository
{
    readonly ISpaceDbContext _context;
    public ClassModuleWorkerRepository(SpaceDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<bool> IsWorkerExist(Guid workerId, List<Guid> moduleIds)
        => await _context.ClassModulesWorkers.AnyAsync(m => m.WorkerId == workerId && moduleIds.Contains(m.ModuleId));
}