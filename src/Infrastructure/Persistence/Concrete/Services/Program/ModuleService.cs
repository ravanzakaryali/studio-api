namespace Space.Infrastructure.Persistence.Concrete.Services;

public class ModuleService : IModuleService
{
    readonly ISpaceDbContext _context;
    public ModuleService(ISpaceDbContext context)
    {
        _context = context;
    }
    public async Task<bool> IsUnique(List<Module> modules)
    {
        List<string> moduleNames = modules.Select(a => a.Name).ToList();
        moduleNames.AddRange(modules.SelectMany(a => a.SubModules?.Select(a => a.Name)!).ToList());
        var a = await _context.Modules.Where(a => moduleNames.Contains(a.Name)).FirstOrDefaultAsync();
        return a != null;
    }
}
