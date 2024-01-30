namespace Space.Application.Handlers;
public class GetNonProgramModulesQuery : IRequest<IEnumerable<GetModuleDto>>
{

}
internal class GetNonProgramModulesHandler : IRequestHandler<GetNonProgramModulesQuery, IEnumerable<GetModuleDto>>
{
    readonly ISpaceDbContext _spaceDbContext;
    public GetNonProgramModulesHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetModuleDto>> Handle(GetNonProgramModulesQuery request, CancellationToken cancellationToken)
    {
        return await _spaceDbContext.Modules
            .Where(m => m.ProgramId == null)
            .Select(m => new GetModuleDto()
            {
                Id = m.Id,
                Hours = m.Hours,
                Name = m.Name,
                Version = m.Version!,
                SubModules = m.SubModules != null ? m.SubModules.Select(sm => new SubModuleDto()
                {
                    Id = sm.Id,
                    Hours = sm.Hours,
                    Name = sm.Name,
                    Version = sm.Version!,
                    TopModuleId = m.Id
                }) : null
            })
            .ToListAsync(cancellationToken);
    }
}