
namespace Space.Application.Handlers;

public class GetModulesByProgramQuery : IRequest<IEnumerable<GetModuleDto>>
{
    public int Id { get; set; }
}
internal class GetModulesByProgramHandler : IRequestHandler<GetModulesByProgramQuery, IEnumerable<GetModuleDto>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetModulesByProgramHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }
    public async Task<IEnumerable<GetModuleDto>> Handle(GetModulesByProgramQuery request, CancellationToken cancellationToken)
    {
        List<Module> modules = await _spaceDbContext.Modules
           .Where(m => m.TopModuleId == null && m.ProgramId == request.Id)
           .Include(m => m.SubModules)
           .ToListAsync(cancellationToken: cancellationToken);

        modules = modules.OrderBy(m => int.Parse(m.Version!)).ToList();

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
            IsSurvey = m.IsSurvey,
            SubModules = m.SubModules?.Select(sm => new SubModuleDto()
            {
                Hours = sm.Hours,
                Id = sm.Id,
                Name = sm.Name,
                IsSurvey = sm.IsSurvey,
                TopModuleId = sm.TopModuleId,
                Version = sm.Version
            })
        });
    }
}
