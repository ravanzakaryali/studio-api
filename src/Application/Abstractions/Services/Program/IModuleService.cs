namespace Space.Application.Abstractions.Services;

public interface IModuleService
{
    Task<bool> IsUnique(List<Module> modules);
}
