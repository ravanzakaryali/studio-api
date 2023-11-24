namespace Space.Application.Abstractions.Services;

public interface IModuleService
{
    Task<bool> IsUnique(List<Module> modules);
    Task<Module?> GetCurrentModuleAsync(Class @class, DateOnly date);
}
