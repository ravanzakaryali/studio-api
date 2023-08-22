namespace Space.Application.Abstractions;

public interface IModuleRepository : IRepository<Module>
{
    Task<bool> IsUnique(List<Module> modules);
}
