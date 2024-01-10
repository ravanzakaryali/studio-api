namespace Space.Application.Handlers.Queries;

public class GetAllProgramsQuery : IRequest<IEnumerable<GetAllProgramResponseDto>>
{

}
public class GetAllProgramsQueryHandler : IRequestHandler<GetAllProgramsQuery, IEnumerable<GetAllProgramResponseDto>>
{
    readonly ISpaceDbContext _spaceDbContext;
    public GetAllProgramsQueryHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetAllProgramResponseDto>> Handle(GetAllProgramsQuery request, CancellationToken cancellationToken)
    {
        List<Program> programs = await _spaceDbContext.Programs
            .Include(c => c.Modules)
            .ToListAsync();
        programs.ToList().ForEach(a => a.Modules = a.Modules.Where(a => a.TopModuleId == null).ToList());
        IEnumerable<GetAllProgramResponseDto> programss = programs.Select(p => new GetAllProgramResponseDto()
        {
            Id = p.Id,
            Name = p.Name,
            TotalHours = p.TotalHours,
            Modules = p.Modules
                .OrderBy(m => Version.TryParse(m.Version, out var parsedVersion) ? parsedVersion : null)
                .Select(p => new GetModuleDto()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Hours = p.Hours,
                    Version = p.Version!,
                    SubModules = p.SubModules?
                    .OrderBy(m => Version.TryParse(m.Version, out var parsedVersion) ? parsedVersion : null)
                    .Select(p => new SubModuleDto()
                    {
                        Id = p.Id,
                        Version = p.Version,
                        Hours = p.Hours,
                        Name = p.Name,
                        TopModuleId = p.TopModuleId
                    })
                })
        });

        return programss;
    }
}
