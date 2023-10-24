using Space.Application.Abstractions;
using Space.Application.Exceptions;
using Space.Domain.Entities;

namespace Space.Infrastructure.Persistence.Concrete;

internal class ModuleRepository : Repository<Module>, IModuleRepository
{
    readonly ISpaceDbContext _context;
    public ModuleRepository(SpaceDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<Module?> GetCurrentModuleAsync(Class @class, DateTime requestDate)
    {
        IEnumerable<ClassSession> classSessionsHour = await _context.ClassSessions
                                                                                    .Where(c => c.ClassId == @class.Id &&
                                                                                                requestDate >= c.Date &&
                                                                                                c.ModuleId != null &&
                                                                                                (c.AttendancesWorkers != null &&
                                                                                                c.AttendancesWorkers.Count != 0))
                                                                                    .Include(c => c.AttendancesWorkers)
                                                                                    .ToListAsync();

        List<Module> modules = @class.Program.Modules
                                                    .OrderBy(m => m.Version)
                                                    .Where(m => m.TopModuleId != null ||
                                                    !m.SubModules.Any())
                                                    .ToList();

        Module? currentModule = null;
        int totalHour = classSessionsHour.Sum(c => c.TotalHour);
        if (totalHour > 0)
        {
            double totalHourModule = 0;

            for (int i = 0; i < modules.Count; i++)
            {
                totalHourModule += modules[i].Hours;
                if (totalHourModule >= totalHour)
                {
                    currentModule = modules[i];
                    break;
                }
            }
            currentModule ??= modules.LastOrDefault();
        }
        else
        {
            currentModule = modules.FirstOrDefault();
        }
        return currentModule;
    }

    public async Task<bool> IsUnique(List<Module> modules)
    {
        List<string> moduleNames = modules.Select(a => a.Name).ToList();
        moduleNames.AddRange(modules.SelectMany(a => a.SubModules.Select(a => a.Name)).ToList());
        var a = await _context.Modules.Where(a => moduleNames.Contains(a.Name)).FirstOrDefaultAsync();
        return a != null;
    }
}
