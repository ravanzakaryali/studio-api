namespace Space.Application.Abstractions;

public interface IModuleRepository : IRepository<Module>
{
    Task<bool> IsUnique(List<Module> modules);
    Task<Module?> GetCurrentModuleAsync(Class @class,DateTime date);
}
