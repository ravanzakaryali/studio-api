namespace Space.Application.Abstractions;

public interface IClassModulesWorkerRepository : IRepository<ClassModulesWorker>
{
    Task<bool> IsWorkerExist(Guid workerId, List<Guid> moduleIds);
}
