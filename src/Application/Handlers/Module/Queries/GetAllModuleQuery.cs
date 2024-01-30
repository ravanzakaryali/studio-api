namespace Space.Application.Handlers.Queries;

public class GetAllModuleQuery : IRequest<IEnumerable<GetModuleDto>>
{
}
internal class GetAllModuleQueryHandler : IRequestHandler<GetAllModuleQuery, IEnumerable<GetModuleDto>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetAllModuleQueryHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetModuleDto>> Handle(GetAllModuleQuery request, CancellationToken cancellationToken)
    {
        List<Module> modules = await _spaceDbContext.Modules
            .Where(m => m.TopModuleId == null)
            .Include(m => m.SubModules)
            .ToListAsync();

        modules = modules.OrderBy(m => Version.TryParse(m.Version, out var parsedVersion) ? parsedVersion : null).ToList();

        foreach (Module module in modules)
        {
            module.SubModules = module.SubModules?
               .OrderBy(m => Version.TryParse(m.Version, out var parsedVersion) ? parsedVersion : null).ToList();
        }

        return modules.Select(m => new GetModuleDto()
        {
            Id = m.Id,
            Hours = m.Hours,
            Name = m.Name,
            Version = m.Version,
            SubModules = m.SubModules?.Select(sm => new SubModuleDto()
            {
                Hours = sm.Hours,
                Id = sm.Id,
                Name = sm.Name,
                TopModuleId = sm.TopModuleId,
                Version = sm.Version
            })
        });
    }
}
